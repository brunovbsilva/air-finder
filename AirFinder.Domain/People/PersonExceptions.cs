﻿using SendGrid.Helpers.Errors.Model;

namespace AirFinder.Domain.People
{
    public class CPFException : MethodNotAllowedException
    { public CPFException() : base("CPF already registered") { } }

    public class EmailException : MethodNotAllowedException
    { public EmailException() : base("Email already registered") { } }

    public class NotFoundPersonException : ArgumentException
    { public NotFoundPersonException() : base("Person not found") { } }

    public class SearchPeopleException : ArgumentException
    { public SearchPeopleException() : base("You have to search with at least 3 characters") { } }
}
