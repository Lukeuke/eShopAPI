# SMTP Service
Service to send emails 

### What it does?

- Sends mails to specified user via gmail SMTP

### Get started

- After cloning make file called:
``settings.json``
- And paste:
```json
{
  "username": "your_gmail_username",
  "password": "your_app_password"
}
```

### How it works?

- GET request

URI:

Type is: 
```c#
public enum MailType
{
    Gmail
}
```
Schema:
```
{{host}}/{type}
```
Example:
```
{{host}}/Gmail
```
Body:
```json
{
  "ToAddress": "example@gmail.com",
  "Subject": "Hello",
  "Body": "<p> World! </p>"
}
```