using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public string User { get; set; }

        [Display(Name="Введите имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [Display(Name = "Введите фамилию")]
        [Required(ErrorMessage = "Не указана фамилия")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Surname { get; set; }

        [Display(Name = "Введите отчество")]
        [Required(ErrorMessage = "Не указано отчество")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string MiddleName { get; set; }

        [Display(Name = "Введите адрес")]
        [Required(ErrorMessage = "Не указан адрес")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Address { get; set; }

        [Display(Name = "Введите номер телефона")]
        [Required(ErrorMessage = "Не указан телефон")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Длина строки должна быть 11 символов")]
        public string Phone { get; set; }

        [Display(Name = "Введите email")]
        [Required(ErrorMessage = "Не указан email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 70 символов")]
        public string Email { get; set; }

        public bool Status { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime OrderTime { get; set; }
        public List<OrderDetail>  OrderDetail { get; set; }

    }
}
