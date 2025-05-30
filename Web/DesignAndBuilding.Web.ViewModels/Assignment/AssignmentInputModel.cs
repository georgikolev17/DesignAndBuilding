﻿namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class AssignmentInputModel
    {
        [Required]
        [Display(Name = "Описание")]
        public IList<IFormFile> Description { get; set; }

        [Required]
        [Display(Name = "Крайна дата")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Предназначено за")]
        public UserType UserType { get; set; }

        [Required]
        public int BuildingId { get; set; }
    }
}
