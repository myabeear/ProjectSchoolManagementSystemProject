using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    // Mendefinisikan ruang nama (namespace) SchoolManagementSystem.Models
    public class CommonFn
    {
        // Membuat kelas CommonFn di dalam namespace SchoolManagementSystem.Models
        public class Commonfnx
        {
            // Mendeklarasikan objek koneksi ke database menggunakan SqlConnection
            // dan menginisialisasi dengan string koneksi yang tersimpan di dalam file konfigurasi (App.config/Web.config) dengan nama "SchoolCS"
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolCS"].ConnectionString);

            // Metode untuk menjalankan perintah SQL yang tidak mengembalikan hasil (misalnya: INSERT, UPDATE, DELETE)
            public void Query(string query)
            {
                // Mengecek apakah koneksi ditutup, jika ya, maka buka koneksi
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                // Membuat objek SqlCommand dengan perintah SQL dan koneksi yang diberikan
                SqlCommand cmd = new SqlCommand(query, con);
                // Menjalankan perintah SQL tanpa mengembalikan hasil (misalnya: INSERT, UPDATE, DELETE)
                cmd.ExecuteNonQuery();
                // Menutup koneksi setelah selesai menjalankan perintah SQL
                con.Close();
            }

            // Metode untuk mengambil data dari database dengan perintah SQL yang mengembalikan hasil (misalnya: SELECT)
            public DataTable Fetch(string query)
            {
                // Mengecek apakah koneksi ditutup, jika ya, maka buka koneksi
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                // Membuat objek SqlCommand dengan perintah SQL dan koneksi yang diberikan
                SqlCommand cmd = new SqlCommand(query, con);
                // Membuat objek SqlDataAdapter untuk mengambil data dari database
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                // Membuat objek DataTable untuk menyimpan data yang diambil dari database
                DataTable dt = new DataTable();
                // Mengisi DataTable dengan data yang diambil menggunakan SqlDataAdapter
                sda.Fill(dt);
                // Mengembalikan DataTable yang berisi data hasil dari perintah SQL
                return dt;
            }
        }
    }
}
