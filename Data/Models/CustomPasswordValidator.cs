using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FurnitureStore.Data.Models
{
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        public int RequiredLength { get; set; } // минимальная длина
        string upList = "ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        string symbolList = @"~!@#$%^&*_-+=`|\(){}[]:;'<>,.?/";
        string numericList = "1234567890";

        public CustomPasswordValidator(int length)
        {
            RequiredLength = length;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (String.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Минимальная длина пароля равна {RequiredLength}"
                });
            }
            if (password.IndexOfAny(upList.ToCharArray())==-1)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Пароль должен содержать хотя бы одну заглавную букву"
                });
            }
            if (password.IndexOfAny(symbolList.ToCharArray()) == -1)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Пароль должен содержать хотя бы один не алфавитно-цифровой символ"
                });
            }
            if (password.IndexOfAny(numericList.ToCharArray()) == -1)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Пароль должен содержать хотя бы одну цифру"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
