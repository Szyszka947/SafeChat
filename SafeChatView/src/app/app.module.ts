import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { UnauthorizedUserLayoutComponent } from './components/unauthorized-user-layout/unauthorized-user-layout.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RegisterFormComponent } from './components/register-form/register-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { HomeComponent } from './components/home/home.component';
import { RefreshTokenComponent } from './components/refresh-token/refresh-token.component';
import { ContactComponent } from './components/contact/contact.component';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MessageComponent } from './components/message/message.component';
import { MatMenuModule } from '@angular/material/menu';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDialogModule } from '@angular/material/dialog';
import { CreateChatDialogComponent } from './dialogs/create-chat-dialog/create-chat-dialog.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { JoinChatDialogComponent } from './dialogs/join-chat-dialog/join-chat-dialog.component';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { LeaveChatDialogComponent } from './dialogs/leave-chat-dialog/leave-chat-dialog.component';
import { InviteUserDialogComponent } from './dialogs/invite-user-dialog/invite-user-dialog.component';
import { KickUserDialogComponent } from './dialogs/kick-user-dialog/kick-user-dialog.component';
import { BanUserDialogComponent } from './dialogs/ban-user-dialog/ban-user-dialog.component';
import { MatBadgeModule } from '@angular/material/badge';
import { MyInvitationsDialogComponent } from './dialogs/my-invitations-dialog/my-invitations-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    UnauthorizedUserLayoutComponent,
    RegisterFormComponent,
    HomeComponent,
    RefreshTokenComponent,
    ContactComponent,
    MessageComponent,
    CreateChatDialogComponent,
    JoinChatDialogComponent,
    LeaveChatDialogComponent,
    InviteUserDialogComponent,
    KickUserDialogComponent,
    BanUserDialogComponent,
    MyInvitationsDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    ReactiveFormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatCardModule,
    MatToolbarModule,
    MatMenuModule,
    MatSlideToggleModule,
    MatDialogModule,
    MatCheckboxModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatBadgeModule
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
