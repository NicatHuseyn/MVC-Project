﻿using BackendProject.Models.Common;

namespace BackendProject.Models;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
	public ICollection<Product> Products { get; set; }

}
