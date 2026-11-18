
using System.Data;
using Microsoft.Data.SqlClient;
using Shared.CategoryDTOs;
using Shared.DTOs;



namespace EcommerceApiDataLayer;

public class CategoryData
{


    public static List<CategoryDTO> GetAllCategories()
    {
        List<CategoryDTO> categoriesList = new List<CategoryDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetAllCategories", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {

                    categoriesList.Add(new CategoryDTO(
                        reader.GetInt32(reader.GetOrdinal("Categories_id")),
                        reader.GetString(reader.GetOrdinal("Categories_name")),
                        reader.GetString(reader.GetOrdinal("Categories_name_ar")),
                        reader.IsDBNull(reader.GetOrdinal("categories_image")) ? null :
                        reader.GetString(reader.GetOrdinal("categories_image"))
                    ));
                }
            }
        }
        return categoriesList;

    }
    public static int AddNewCategory(string name, string name_ar, string imageName)
    {
        int categoryId = -1;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_addNewCategory", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Name_ar", name_ar);
            cmd.Parameters.AddWithValue("@ImageName", imageName);

            var categoryId_param = new SqlParameter("@CategoryId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(categoryId_param);

            connection.Open();
            cmd.ExecuteNonQuery();

            if (categoryId_param.Value != null && categoryId_param.Value != DBNull.Value)
            {
                categoryId = Convert.ToInt32(categoryId_param.Value);
            }

        }
        return categoryId;

    }
    public static bool UpdateCategroy(CategoryDTO catDto)
    {
        bool isUpdate = false;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_updateCategory", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CategoryId", catDto.Id);
            cmd.Parameters.AddWithValue("@CategoryName", catDto.Name);
            cmd.Parameters.AddWithValue("@CategoryNameAr", catDto.Name_ar);
            cmd.Parameters.AddWithValue("@CategoryImageName", catDto.image);

            var isUpdate_param = new SqlParameter("@isUpdate", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(isUpdate_param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

                if (isUpdate_param.Value != null && isUpdate_param.Value != DBNull.Value)
                    isUpdate = Convert.ToBoolean(isUpdate_param.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isUpdate = false;
            }

        }
        return isUpdate;
    }
    public static bool findCategorybyId(ref CategoryDTO catDto, ref DateTime CategoryCreated)
    {
        bool isFound = false;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_findCategoryById", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryId", catDto.Id);

            connection.Open();

            try
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        catDto.Name = reader.GetString(reader.GetOrdinal("categories_name"));
                        catDto.Name_ar = reader.GetString(reader.GetOrdinal("categories_name_ar"));
                        catDto.image = reader.GetString(reader.GetOrdinal("categories_image"));
                        CategoryCreated = reader.GetDateTime(reader.GetOrdinal("categories_datetime"));
                    }
                    else
                        isFound = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }

        }
        return isFound;
    }
    public static bool DeletCategoryById(int categoryId)
    {
        bool isDelete = false;
        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_admin_deleteCategory", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            var isDeleted_param = new SqlParameter("@isDeleted", SqlDbType.Bit)
            {
                Direction= ParameterDirection.Output
            };
            cmd.Parameters.Add(isDeleted_param);
            connection.Open();
            cmd.ExecuteNonQuery();
            if(isDeleted_param.Value != null && isDeleted_param.Value != DBNull.Value)
                isDelete = Convert.ToBoolean(isDeleted_param.Value);
        }
        return isDelete;
    }
}