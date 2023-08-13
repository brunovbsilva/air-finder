using SendGrid.Helpers.Errors.Model;

namespace AirFinder.Domain.People
{
    public class CPFException : MethodNotAllowedException
    { public CPFException() : base("CPF already registered") { } }

    public class EmailException : MethodNotAllowedException
    { public EmailException() : base("Email already registered") { } }

    public class NotFoundEmailException : Exception
    { public NotFoundEmailException() : base("Email not found") { } }

    public class NotFoundCPFException : Exception
    { public NotFoundCPFException() : base("CPF not found") { } }

    public class NotFoundPersonException : Exception
    { public NotFoundPersonException() : base("Person not found") { } }
}
