using System.Net;
using System.Net.Mail;

namespace Company.G03.PL.Helpers
{
	public static class EmailSettings
	{

		public static bool SendEmail(Email email) {
			//Mall Server: Gmail

			// SMTP Simple mail Transfer protocol
			try
			{
				var client = new SmtpClient("smtp.gmail.com", 587);
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential("ezzsaleh777@gmail.com", "zjfwtcmkjwgdizmi");
				//hqscaugshlhbtbnx -> not working
				//zjfw tcmk jwgd izmi
				client.Send("ezzsaleh777@gmail.com", email.To, email.Subject, email.Body);
				return true;
			}
			catch (Exception ex) { 
			return false;
			}
		
		}
	}
}
