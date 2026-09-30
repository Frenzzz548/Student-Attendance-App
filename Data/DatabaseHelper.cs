using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Data.Sqlite;
using StudentAttendanceApp.Models;

namespace StudentAttendanceApp.Data
{
    /// <summary>
    /// Semua akses database SQLite ada di sini (CRUD + seed data dummy).
    /// File database "attendance.db" otomatis dibuat di folder output aplikasi.
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>Daftar mata kuliah (dipakai ComboBox dan data dummy).</summary>
        public static readonly string[] Courses =
        {
            "Pemrogramman Berbasis Kerangka Kerja",
            "Keamanan Informasi",
            "Pemodelan dan Simulasi",
            "Rekayasa Sistem Berbasis Pengetahuan",
            "Grafika Komputer",
            "Pengolahan Citra dan Visi Komputer",
            "Sistem Terdistribusi"
        };

        private static readonly string DbPath =
            Path.Combine(AppContext.BaseDirectory, "attendance.db");

        private static readonly string ConnectionString =
            new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();

        private const string DateFormat = "yyyy-MM-dd";

        // ------------------------------------------------------------
        // INISIALISASI
        // ------------------------------------------------------------

        /// <summary>Membuat tabel jika belum ada, lalu mengisi 20 data dummy jika masih kosong.</summary>
        public static void Initialize()
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Attendance (
                            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nrp         TEXT NOT NULL,
                            Nama        TEXT NOT NULL,
                            MataKuliah  TEXT NOT NULL,
                            Tanggal     TEXT NOT NULL,
                            Status      TEXT NOT NULL,
                            Keterangan  TEXT
                        );";
                    cmd.ExecuteNonQuery();
                }

                // Migrasi database lama: jika database sebelumnya masih memakai kolom Nim,
                // ubah nama kolom tersebut menjadi Nrp agar data lama tetap dapat digunakan.
                bool hasNim = false;
                bool hasNrp = false;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "PRAGMA table_info(Attendance);";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader.GetString(1);
                            if (columnName == "Nim") hasNim = true;
                            if (columnName == "Nrp") hasNrp = true;
                        }
                    }
                }

                if (hasNim && !hasNrp)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "ALTER TABLE Attendance RENAME COLUMN Nim TO Nrp;";
                        cmd.ExecuteNonQuery();
                    }
                }

                long count;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Attendance;";
                    count = Convert.ToInt64(cmd.ExecuteScalar());
                }

                if (count == 0)
                {
                    SeedDummyData(conn);
                }
            }
        }

        /// <summary>Mengisi 20 data dummy. Tanggal dibuat relatif terhadap hari ini.</summary>
        private static void SeedDummyData(SqliteConnection conn)
        {
            // nrp, nama, index mata kuliah, selisih hari dari hari ini, status, keterangan
            var seed = new (string Nrp, string Nama, int Course, int DayOffset, string Status, string Ket)[]
            {
                ("5025241001", "Ahmad Fauzi",         0, -14, "Hadir", ""),
                ("5025241002", "Bella Kusuma",        0, -14, "Hadir", ""),
                ("5025241003", "Citra Ayu Lestari",   1, -13, "Izin",  "Mengurus administrasi beasiswa"),
                ("5025241004", "Dimas Prasetyo",      1, -13, "Hadir", ""),
                ("5025241005", "Eka Putri Wulandari", 2, -12, "Sakit", "Demam, ada surat dokter"),
                ("5025241006", "Fajar Nugroho",       2, -12, "Hadir", ""),
                ("5025241007", "Galih Ramadhan",      3, -11, "Alpha", ""),
                ("5025241008", "Hana Safitri",        3, -11, "Hadir", ""),
                ("5025241009", "Irfan Hakim",         4, -10, "Hadir", ""),
                ("5025241010", "Jihan Maharani",      4, -10, "Izin",  "Mengikuti lomba tingkat nasional"),
                ("5025241011", "Kevin Aditya",        5,  -9, "Hadir", ""),
                ("5025241012", "Laila Nur Azizah",    5,  -9, "Sakit", "Flu dan batuk"),
                ("5025241013", "Muhammad Rizky",      6,  -8, "Hadir", ""),
                ("5025241014", "Nadia Permata",       6,  -8, "Alpha", ""),
                ("5025241015", "Oktavianus Putra",    0,  -7, "Hadir", ""),
                ("5025241016", "Putri Anggraini",     1,  -6, "Hadir", ""),
                ("5025241017", "Rangga Wijaya",       2,  -5, "Izin",  "Acara keluarga"),
                ("5025241018", "Salsabila Amalia",    3,  -4, "Hadir", ""),
                ("5025241019", "Tegar Saputra",       4,  -3, "Alpha", ""),
                ("5025241020", "Vina Oktaviani",      5,  -2, "Hadir", "")
            };

            using (var tx = conn.BeginTransaction())
            {
                foreach (var s in seed)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tx;
                        cmd.CommandText = @"
                            INSERT INTO Attendance (Nrp, Nama, MataKuliah, Tanggal, Status, Keterangan)
                            VALUES (@nrp, @nama, @mk, @tgl, @status, @ket);";
                        cmd.Parameters.AddWithValue("@nrp", s.Nrp);
                        cmd.Parameters.AddWithValue("@nama", s.Nama);
                        cmd.Parameters.AddWithValue("@mk", Courses[s.Course]);
                        cmd.Parameters.AddWithValue("@tgl",
                            DateTime.Today.AddDays(s.DayOffset).ToString(DateFormat, CultureInfo.InvariantCulture));
                        cmd.Parameters.AddWithValue("@status", s.Status);
                        cmd.Parameters.AddWithValue("@ket", s.Ket);
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
        }

        // ------------------------------------------------------------
        // READ
        // ------------------------------------------------------------

        /// <summary>
        /// Mengambil data absensi. keyword mencari di NRP / Nama / Mata Kuliah,
        /// status memfilter status kehadiran (kosong = semua).
        /// </summary>
        public static List<AttendanceRecord> GetAll(string keyword, string status)
        {
            var list = new List<AttendanceRecord>();
            keyword = keyword ?? "";
            status = status ?? "";

            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Nrp, Nama, MataKuliah, Tanggal, Status, Keterangan
                        FROM Attendance
                        WHERE (@kw = '' OR Nrp LIKE @like OR Nama LIKE @like OR MataKuliah LIKE @like)
                          AND (@status = '' OR Status = @status)
                        ORDER BY Tanggal DESC, Id DESC;";
                    cmd.Parameters.AddWithValue("@kw", keyword);
                    cmd.Parameters.AddWithValue("@like", "%" + keyword + "%");
                    cmd.Parameters.AddWithValue("@status", status);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new AttendanceRecord
                            {
                                Id = reader.GetInt32(0),
                                Nrp = reader.GetString(1),
                                Nama = reader.GetString(2),
                                MataKuliah = reader.GetString(3),
                                Tanggal = DateTime.ParseExact(reader.GetString(4), DateFormat,
                                              CultureInfo.InvariantCulture),
                                Status = reader.GetString(5),
                                Keterangan = reader.IsDBNull(6) ? "" : reader.GetString(6)
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>Cek apakah mahasiswa yang sama sudah diabsen di mata kuliah + tanggal yang sama.</summary>
        public static bool IsDuplicate(string nrp, string mataKuliah, DateTime tanggal, int? excludeId)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT COUNT(*) FROM Attendance
                        WHERE Nrp = @nrp AND MataKuliah = @mk AND Tanggal = @tgl AND Id <> @id;";
                    cmd.Parameters.AddWithValue("@nrp", nrp);
                    cmd.Parameters.AddWithValue("@mk", mataKuliah);
                    cmd.Parameters.AddWithValue("@tgl", tanggal.ToString(DateFormat, CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@id", excludeId ?? 0);
                    return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        // ------------------------------------------------------------
        // CREATE / UPDATE / DELETE
        // ------------------------------------------------------------

        public static void Insert(AttendanceRecord r)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Attendance (Nrp, Nama, MataKuliah, Tanggal, Status, Keterangan)
                        VALUES (@nrp, @nama, @mk, @tgl, @status, @ket);";
                    AddRecordParameters(cmd, r);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Update(AttendanceRecord r)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Attendance
                        SET Nrp = @nrp, Nama = @nama, MataKuliah = @mk,
                            Tanggal = @tgl, Status = @status, Keterangan = @ket
                        WHERE Id = @id;";
                    AddRecordParameters(cmd, r);
                    cmd.Parameters.AddWithValue("@id", r.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(int id)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Attendance WHERE Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void AddRecordParameters(SqliteCommand cmd, AttendanceRecord r)
        {
            cmd.Parameters.AddWithValue("@nrp", r.Nrp);
            cmd.Parameters.AddWithValue("@nama", r.Nama);
            cmd.Parameters.AddWithValue("@mk", r.MataKuliah);
            cmd.Parameters.AddWithValue("@tgl", r.Tanggal.ToString(DateFormat, CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@status", r.Status);
            cmd.Parameters.AddWithValue("@ket", r.Keterangan ?? "");
        }
    }
}
