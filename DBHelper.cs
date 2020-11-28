using MySql.Data.MySqlClient;
using OneNX.ASP.NET.MVC.Validation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneNX.ASP.NET.MVC.Validation
{
	public class DBHelper
	{
		private const string connectionStr = "data source=localhost;database=apc_web;user id=root;password=root;SslMode=none;charset=utf8";

		public static long Create(FileModel model)
		{
			long newId = 0;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionStr))
				{
					var createSql = $"insert into file_meta(FileType, FileName, UploadDate) values('{model.FileType}', '{model.FileName}', '{model.UploadDate}');";
					MySqlCommand createCommand = new MySqlCommand(createSql, conn);
					conn.Open();
					createCommand.ExecuteNonQuery();
					newId = createCommand.LastInsertedId;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return newId;
		}

		public static FileModel GetById(int id) {
			var model = GetAll($"where FileId='{id}'").FirstOrDefault();
			return model;
		}

		public static FileModel GetByFileName(string fileName)
		{
			var model = GetAll($"where FileName='{fileName}'").FirstOrDefault();
			return model;
		}

		public static List<FileModel> GetAll(string condition = "where 1=1") 
		{
			List<FileModel> models = new List<FileModel>();
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionStr))
				{
					var selectSql = $"select * from apc_web.file_meta {condition};";
					MySqlCommand selectCommand = new MySqlCommand(selectSql, conn);
					conn.Open();
					MySqlDataReader reader = selectCommand.ExecuteReader();
					while (reader.Read())
					{
						var fileId = long.Parse(reader["FileId"]?.ToString());
						var fileType = reader["FileType"]?.ToString();
						var fileName = reader["FileName"]?.ToString();
						var uploadDate = reader["UploadDate"]?.ToString();
						var subrecipeGenStatus = reader["SubrecipeGenStatus"]?.ToString();
						var subrecipeName = reader["SubrecipeName"]?.ToString();
						var transferToRMSStatus = reader["TransferToRMSStatus"]?.ToString();
						models.Add(new FileModel()
						{
							FileId = fileId,
							FileType = fileType,
							FileName = fileName,
							UploadDate = uploadDate,
							SubrecipeGenStatus = subrecipeGenStatus,
							SubrecipeName = subrecipeName,
							TransferToRMSStatus = transferToRMSStatus,
						});
					}
				}
				return models;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static int Update(FileModel model)
		{
			int effectRow = 0;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionStr))
				{
					var updateSql = $"update file_meta set SubrecipeGenStatus='{model.SubrecipeGenStatus}', TransferToRMSStatus='{model.TransferToRMSStatus}', SubrecipeName='{model.SubrecipeName}' where FileId={model.FileId};";
					MySqlCommand updateCommand = new MySqlCommand(updateSql, conn);
					conn.Open();
					effectRow = updateCommand.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return effectRow;
		}
	}
}