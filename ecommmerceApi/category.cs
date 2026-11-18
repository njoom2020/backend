


using System.Security.Cryptography.Pkcs;
using Shared.DTOs;
using EcommerceApiDataLayer;
using Shared.CategoryDTOs;



namespace EcommerceApiBusinessLayer;

public class Category
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;

    public CategoryDTO CDTO
    {
        get
        {
            return new CategoryDTO(this.Id, this.Name, this.Name_ar, this.image);
        }
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Name_ar { get; set; }
    public string image { get; set; }
    public DateTime? Created { get; set; } = DateTime.UtcNow;
    public Category(CategoryDTO CTDO, DateTime? created, enMode cMode = enMode.AddNew)
    {
        this.Id = CTDO.Id;
        this.Name = CTDO.Name;
        this.Name_ar = CTDO.Name_ar;
        this.image = CTDO.image ?? "noImage.svg";
        this.Created = created;
        Mode = cMode;
    }
    public static List<CategoryDTO> GetAllCategories(string? baseUrl = null)
    {
        var categories = CategoryData.GetAllCategories();

        if (string.IsNullOrWhiteSpace(baseUrl))
            return categories;

        foreach (var cat in categories)
            if (!string.IsNullOrWhiteSpace(cat.image))
                cat.image = $"{baseUrl}/api/CategoryApi/image/{cat.image}";

        return categories;
    }

    private bool _AddNew()
    {
        this.Id = CategoryData.AddNewCategory(this.Name, this.Name_ar, this.image);
        return this.Id > 0;
    }

    private bool _Update()
    {
        return CategoryData.UpdateCategroy(this.CDTO);
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                Mode = enMode.Update;
                return _AddNew();
            case enMode.Update:
            return _Update();
        }
        return false;
    }

    public static Category? FindCategoryById(int categoryId)
    {
        CategoryDTO catDto  = new CategoryDTO(categoryId, "", "", "");
        DateTime created = DateTime.UtcNow;
        bool isFind = CategoryData.findCategorybyId(ref catDto,ref created);
        if(isFind)
            return new Category(catDto, created, enMode.Update);
        else
            return null;
    }

    public static bool DeleteCategoryById(int categoryId)
    {
        return CategoryData.DeletCategoryById(categoryId);
    }
}