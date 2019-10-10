using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using TradeTracker.Models;

namespace TradeTracker.ApiControllers
{
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {

        public HttpResponseMessage Post(Email email)
        {
            Object json;
            HttpStatusCode code;

            if (email == null)
            {
                email = new Email();
            }

            var emailValidation = new ValidationContext(email);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(email, emailValidation, results, true))
            {
                var errors = new[] {
                    new { Error = email.Recipient == null, Message ="a recipent" },
                    new { Error = email.Subject == null, Message ="a subject" },
                    new { Error = email.Body == null, Message ="a message" }
                };

                List<string> errorsList = new List<string>();
                for (int i = 0; i < errors.Length; i++)
                {
                    if (errors[i].Error)
                    {
                        errorsList.Add(errors[i].Message);
                    }
                }
                System.Diagnostics.Debug.WriteLine(errorsList.ToString());
                if (errorsList.Count() > 0)
                {
                    string response = "Please specify ";
                    for (int i = 0; i < errorsList.Count(); i++)
                    {
                        if (i + 1 == errorsList.Count())
                        {
                            response += errorsList[i] + ".";
                        }
                        else if (i == errorsList.Count() - 2)
                        {
                            response += errorsList[i] + ", and ";
                        }
                        else
                        {
                            response += errorsList[i] + ", ";
                        }
                    }
                    json = new { Message = response, Errors = results };
                }
                else
                {
                    json = new { Message = "The request is invalid due to bad input.", Errors = results };
                }
                code = HttpStatusCode.BadRequest;

            }
            else
            {
                try
                {
                    code = HttpStatusCode.Created;
                    json = new { Message = "Email Successfully Sent!" };
                    string adminEmail = "contact.tradetracker@gmail.com";
                    string adminPassword = "Jc12345*";
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(adminEmail, adminPassword);
                    string message = $"New email from: {email.Recipient}.\nMessage: {email.Body}";
                    MailMessage mm = new MailMessage(adminEmail, adminEmail, email.Subject, message);
                    client.Send(mm);
                }
                catch (Exception e)
                {
                    json = new { Message = "Failed to send email.", Errors = e };
                    code = HttpStatusCode.InternalServerError;
                }


            }
            return Request.CreateResponse(code, json, JsonMediaTypeFormatter.DefaultMediaType);
        }

    }
}
