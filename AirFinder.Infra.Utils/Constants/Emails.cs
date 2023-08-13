namespace AirFinder.Infra.Utils.Constants
{
    public static class Emails
    {
		public const string ForgotPasswordSubject = "Seu token para alteração de senha";
		public const string ForgotPasswordEmail = @"<!DOCTYPE html>
            <html>
            <head>
	            <meta charset=""UTF-8"">
	            <title>Seu token para alteração de senha</title>
            </head>
            <body>
	            <h1>Seu token para alteração de senha</h1>
	            <p>Prezado(a) {0},</p>
	            <p>Recebemos sua solicitação de alteração de senha para sua conta. Para prosseguir com a alteração de senha, utilize o seguinte token de 6 dígitos:</p>
	            <p style=""font-size: 24px; font-weight: bold;"">{1}</p>
	            <p>Este token é válido por 30 minutos a partir do recebimento deste e-mail. Por favor, não compartilhe este token com ninguém, pois é exclusivo para você. Após a expiração do prazo, o token não poderá ser utilizado e você precisará solicitar um novo token de alteração de senha.</p>
	            <p>Se você não solicitou esta alteração de senha, por favor, ignore este e-mail e entre em contato conosco imediatamente para proteger sua conta.</p>
	            <p>Se tiver alguma dúvida ou preocupação, por favor, não hesite em nos contatar.</p>
	            <br>
	            <p>Atenciosamente,</p>
	            <p>Air Finder</p>
            </body>
            </html>";
}
}
