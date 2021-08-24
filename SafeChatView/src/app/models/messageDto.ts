export class MessageDto {
    public groupId!: number;
    public senderName!: string;
    public dateTime!: string;
    public content!: string;
    public imageUrls!: Array<string>;
    public messageType!: number;
}