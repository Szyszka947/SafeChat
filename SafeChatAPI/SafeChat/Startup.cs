using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using SafeChatAPI.Hubs;
using SafeChatAPI.Models;
using SafeChatAPI.Services.Cookies;
using SafeChatAPI.Services.Files;
using SafeChatAPI.Services.GroupAdmins;
using SafeChatAPI.Services.GroupBans;
using SafeChatAPI.Services.GroupInvitations;
using SafeChatAPI.Services.Groups;
using SafeChatAPI.Services.JWT;
using SafeChatAPI.Services.Messages;
using SafeChatAPI.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeChatAPI
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<AppDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddSignalR(cfg =>
            {
                cfg.MaximumReceiveMessageSize = 15000;
            });

            services.AddScoped<GetIsEmailTakenService>();
            services.AddScoped<GetIsUserNameTakenService>();
            services.AddScoped<FindUserByEmailService>();
            services.AddScoped<FindUserByUserNameService>();
            services.AddScoped<CreateUserService>();
            services.AddScoped<GenerateAccessTokenService>();
            services.AddScoped<RegisterUserService>();
            services.AddScoped<LogInUserService>();
            services.AddScoped<GetUserInfoFromAccessTokenService>();
            services.AddScoped<GenerateRefreshTokenService>();
            services.AddScoped<ValidateRefreshTokenService>();
            services.AddScoped<ValidateAccessTokenService>();
            services.AddScoped<CookieExistsService>();
            services.AddScoped<GetCookieService>();
            services.AddScoped<FindUserByRefreshTokenService>();
            services.AddScoped<ValidateMessageService>();
            services.AddScoped<GetGroupsWhereUserIsMemberOfService>();
            services.AddScoped<IsUserInGroupService>();
            services.AddScoped<FindGroupByIdService>();
            services.AddScoped<FindUserByIdService>();
            services.AddScoped<GetGroupMembersService>();
            services.AddScoped<GetGroupMessagesService>();
            services.AddScoped<SaveMessageService>();
            services.AddScoped<SaveFileService>();
            services.AddScoped<GetGroupImagesService>();
            services.AddScoped<GetImagesByMessageIdService>();
            services.AddScoped<IsPublicGroupNameTakenService>();
            services.AddScoped<CreateGroupService>();
            services.AddScoped<GetAllPublicGroupsService>();
            services.AddScoped<JoinGroupService>();
            services.AddScoped<LeaveGroupService>();
            services.AddScoped<IsUserBannedForGroupService>();
            services.AddScoped<IsUserAdminInGroupService>();
            services.AddScoped<AddGroupAdminService>();
            services.AddScoped<IsUserAdminInGroupService>();
            services.AddScoped<AddGroupInviteService>();
            services.AddScoped<RemoveGroupAdminService>();
            services.AddScoped<BanUserService>();
            services.AddScoped<GetUsersWhoNameStartsWithService>();
            services.AddScoped<GetGroupBanService>();
            services.AddScoped<IsUserInvitedToGroupService>();
            services.AddScoped<GetGroupInvitationsService>();
            services.AddScoped<RemoveGroupInvitationService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
                AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidAudience = JWTConfig.ValidAudience,
                        ValidIssuer = JWTConfig.ValidIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTConfig.SecretKey)),
                        ClockSkew = TimeSpan.FromSeconds(10),
                        NameClaimType = JwtClaimTypes.NickName,
                    };
                });

            services.AddCors(cfg =>
            {
                cfg.AddDefaultPolicy(cfgPolicy =>
                {
                    cfgPolicy.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(p => true)
                    .AllowCredentials();
                });
            });

            services.PostConfigure<ApiBehaviorOptions>(cfg =>
            {
                cfg.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(new ApiResponse
                    {
                        Result = actionContext.ModelState.Keys.ToList().
                            Zip(actionContext.ModelState.Values.Select(p => p.Errors.Select(x => x.ErrorMessage).ToList()),
                                (key, errors) => new { key, errors }).ToDictionary(k => k.key, e => e.errors)
                    });
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                var accessToken = context.Request.Cookies["accessToken"];

                context.Request.Headers["Authorization"] = "Bearer " + accessToken;

                await next();
            });

            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub", cfg =>
                {
                    cfg.Transports = HttpTransportType.WebSockets;
                });
            });
        }
    }
}
