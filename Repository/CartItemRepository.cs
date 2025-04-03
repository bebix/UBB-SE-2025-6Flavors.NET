﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutApp.Models;

namespace WorkoutApp.Repository
{
    public class CartItemRepository
    {
        private string connectionString = @"Server=Dell\SQLEXPRESS;Database=ShopDB;Integrated Security=True;TrustServerCertificate=True";
        private SqlConnection connection;

        public CartItemRepository()
        {
            this.connection = new SqlConnection(connectionString);
        }

        private int GetActiveCartId()
        {
            return 1;
        }

        public List<CartItem> GetAll()
        {
            List<CartItem> list = new List<CartItem>();
            list.Add(new CartItem { Id = 0, CartId = 1, ProductId = 0, Quantity = 2 });
            list.Add(new CartItem { Id = 1, CartId = 1, ProductId = 1, Quantity = 1 });
            list.Add(new CartItem { Id = 2, CartId = 1, ProductId = 2, Quantity = 3 });
            return list;

            connection.Open();
            List<CartItem> cartItems = new List<CartItem>();

            int cartId = GetActiveCartId();

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM CartItem Where IsActive = 1 and CartId = " + cartId.ToString(), connection);
            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                CartItem cartItem = new CartItem
                {
                    Id = reader.GetInt64(reader.GetOrdinal("ID")),
                    CartId = reader.GetInt64(reader.GetOrdinal("CartId")),
                    ProductId = reader.GetInt64(reader.GetOrdinal("ProductID")),
                    Quantity = reader.GetInt64(reader.GetOrdinal("Quantity")),
                };
                cartItems.Add(cartItem);
            }

            reader.Close();
            connection.Close();

            return cartItems;
        }

        public void AddCartItem(int productId, int quantity)
        {
            connection.Open();

            SqlCommand insertCommand = new SqlCommand(
                "INSERT INTO CartItem (Id, CartId, ProductId, Quantity, IsActive) VALUES (@Id, @CartId, @ProductId, @Quantity, @IsActive)",
                connection
            );

            SqlCommand getMaxIdCommand = new SqlCommand("SELECT ISNULL(MAX(ID), 0) + 1 FROM CartItem", connection);
            int newId = (int)getMaxIdCommand.ExecuteScalar();

            insertCommand.Parameters.AddWithValue("@Id", newId);
            insertCommand.Parameters.AddWithValue("@CartId", this.GetActiveCartId());
            insertCommand.Parameters.AddWithValue("@ProductId", productId);
            insertCommand.Parameters.AddWithValue("@Quantity", quantity);
            insertCommand.Parameters.AddWithValue("@IsActive", true);

            insertCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateById(long id, long newQuantity)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE CartItem SET Quantity = @Quantity WHERE Id = @Id",
                connection
            );

            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@Quantity", newQuantity);

            updateCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void DeleteById(long id)
        {
            connection.Open();

            SqlCommand updateCommand = new SqlCommand(
                "UPDATE CartItem SET IsActive = false WHERE Id = @Id",
                connection
            );

            updateCommand.Parameters.AddWithValue("@Id", id);

            updateCommand.ExecuteNonQuery();

            connection.Close();
        }

        public CartItem GetItemById(long id)
        {
            throw new NotImplementedException();
        }
    }
}