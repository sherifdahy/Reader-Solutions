using FluentValidation;
using Reader.BLL.Contracts.Auth.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOTE.Solutions.BLL.Contracts.Auth.Validations;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("required email, dahy")
            .EmailAddress()
            .WithMessage("email not valid, dahy");

        RuleFor(x => x.Password).NotEmpty();
    }
}
