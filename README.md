# Student Attendance App

Aplikasi desktop untuk mengelola data absensi mahasiswa menggunakan **C# WPF (.NET 8)** dan **SQLite**.

---

# Identitas

| Data | Isi |
|---|---|
| **Nama** | Bagus Cahya Saputra |
| **NRP** | 5025241067 |
| **Kelas** | PBKK-C |

---

# 1. Deskripsi Project

**Student Attendance App** merupakan aplikasi desktop berbasis WPF yang digunakan untuk mencatat dan mengelola data kehadiran mahasiswa.

Aplikasi menyediakan satu halaman utama yang terdiri dari form input absensi dan tabel data absensi. Pengguna dapat menambahkan data baru, melihat data, mencari data, memfilter berdasarkan status kehadiran, mengubah data, serta menghapus data.

Data absensi disimpan secara lokal menggunakan **SQLite**, sehingga aplikasi tidak membutuhkan database server terpisah.

Fitur utama aplikasi:

- Menampilkan data absensi mahasiswa.
- Menambahkan data absensi.
- Mengubah data absensi.
- Menghapus data absensi.
- Validasi data sebelum disimpan.
- Pencegahan data absensi duplikat.
- Pencarian berdasarkan NRP, nama, atau mata kuliah.
- Filter berdasarkan status kehadiran.
- Menampilkan ringkasan jumlah data dan persentase kehadiran.
- Menyediakan data dummy awal untuk mempermudah pengujian.

---

# 2. Teknologi yang Digunakan

| Teknologi | Penggunaan |
|---|---|
| **C#** | Bahasa pemrograman utama |
| **WPF** | Framework untuk membangun antarmuka desktop |
| **.NET 8** | Target framework aplikasi |
| **XAML** | Membuat struktur dan tampilan antarmuka |
| **SQLite** | Penyimpanan data absensi |
| **Microsoft.Data.Sqlite** | Library untuk menghubungkan aplikasi dengan SQLite |

Package database yang digunakan:

```text
Microsoft.Data.Sqlite 8.0.11
```

---

# 3. Struktur Project

```text
StudentAttendanceApp
│
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── StudentAttendanceApp.csproj
│
├── Models
│   └── AttendanceRecord.cs
│
└── Data
    └── DatabaseHelper.cs
```

---

# 4. Penjelasan Setiap Bagian Project

## 4.1 `StudentAttendanceApp.csproj`

File ini merupakan konfigurasi utama project.

Bagian pentingnya:

- Menentukan aplikasi sebagai `WinExe`.
- Menggunakan target framework `.NET 8.0-windows`.
- Mengaktifkan WPF.
- Menentukan namespace utama aplikasi.
- Mendefinisikan package `Microsoft.Data.Sqlite`.

Dengan demikian, project dapat menggunakan komponen WPF sekaligus melakukan komunikasi dengan database SQLite.

---

## 4.2 `App.xaml`

`App.xaml` digunakan untuk konfigurasi awal aplikasi WPF.

Pada project ini, `StartupUri` diarahkan ke:

```text
MainWindow.xaml
```

Artinya, ketika aplikasi dijalankan, window utama yang pertama kali dibuka adalah `MainWindow`.

---

## 4.3 `App.xaml.cs`

File ini merupakan code-behind untuk `App.xaml`.

Pada project ini, class `App` mewarisi:

```csharp
Application
```

Tidak terdapat proses bisnis khusus di dalam file ini karena proses utama aplikasi berada pada `MainWindow.xaml.cs` dan `DatabaseHelper.cs`.

---

# 5. Bagian Antarmuka `MainWindow.xaml`

<img width="959" height="606" alt="image" src="https://github.com/user-attachments/assets/dfc038bf-d96e-4b75-90f5-f632a17c74f3" />

`MainWindow.xaml` merupakan bagian yang mendefinisikan seluruh tampilan aplikasi.

Secara umum halaman dibagi menjadi dua bagian:

1. **Form Input Absensi**
2. **Data Absensi**

## 5.1 Form Input Absensi

<img width="267" height="513" alt="image" src="https://github.com/user-attachments/assets/5c096caa-8a6e-4f81-9482-1047bab0300c" />

Bagian form digunakan untuk memasukkan atau mengubah data absensi.

Input yang tersedia:

### NRP

Menggunakan `TextBox`.

NRP memiliki validasi:

- Wajib diisi.
- Hanya boleh berisi angka.
- Panjang 8–12 digit.

### Nama

Menggunakan `TextBox`.

Validasi:

- Wajib diisi.
- Minimal 3 karakter.

### Mata Kuliah

Menggunakan `ComboBox`.

Pilihan mata kuliah berasal dari daftar yang didefinisikan pada:

```text
DatabaseHelper.Courses
```

Daftar tersebut meliputi:

- Pemrogramman Berbasis Kerangka Kerja
- Keamanan Informasi
- Pemodelan dan Simulasi
- Rekayasa Sistem Berbasis Pengetahuan
- Grafika Komputer
- Pengolahan Citra dan Visi Komputer
- Sistem Terdistribusi

### Tanggal

Menggunakan `DatePicker`.

Tanggal absensi:

- Wajib dipilih.
- Tidak boleh melebihi tanggal hari ini.

### Status Kehadiran

Menggunakan `RadioButton`.

Pilihan status:

- Hadir
- Izin
- Sakit
- Alpha

### Keterangan

Menggunakan `TextBox` multiline.

Keterangan wajib diisi apabila status yang dipilih adalah:

```text
Izin
```

atau

```text
Sakit
```

---

# 6. Tombol pada Form

<img width="239" height="76" alt="image" src="https://github.com/user-attachments/assets/58bb8f95-0dd0-4085-9450-1488edcb0f79" />

## Simpan

Digunakan untuk menambahkan data absensi baru ke database.

Sebelum data disimpan, aplikasi menjalankan seluruh validasi input.

Jika valid, data dimasukkan menggunakan operasi:

```text
INSERT
```

---

## Update

Digunakan untuk mengubah data yang dipilih pada tabel.

Tombol ini awalnya nonaktif.

Tombol akan aktif setelah pengguna memilih satu baris pada `DataGrid`.

Data yang telah dipilih akan dimasukkan kembali ke form sehingga pengguna dapat mengubahnya.

---

## Reset

Mengembalikan form ke kondisi awal.

Form akan:

- Mengosongkan NRP.
- Mengosongkan nama.
- Menghapus pilihan mata kuliah.
- Mengembalikan tanggal ke hari ini.
- Mengatur status menjadi `Hadir`.
- Mengosongkan keterangan.
- Menghilangkan pilihan data pada tabel.
- Menonaktifkan tombol Update dan Hapus.

---

## Hapus

Digunakan untuk menghapus data yang sedang dipilih.

Sebelum data benar-benar dihapus, aplikasi menampilkan konfirmasi:

```text
Yakin ingin menghapus data absensi ini?
```

Data hanya dihapus apabila pengguna memilih:

```text
Yes
```

---

# 7. DataGrid Absensi

<img width="617" height="38" alt="image" src="https://github.com/user-attachments/assets/dee51f20-af7c-41d7-a25a-6d28218a6254" />

Data absensi ditampilkan menggunakan `DataGrid`.

Kolom yang ditampilkan:

| Kolom | Isi |
|---|---|
| NRP | Nomor induk mahasiswa |
| Nama | Nama mahasiswa |
| Mata Kuliah | Mata kuliah absensi |
| Tanggal | Tanggal absensi |
| Status | Status kehadiran |
| Keterangan | Catatan tambahan |

Warna teks status dibedakan untuk memudahkan pembacaan:

- **Hadir**
- **Izin**
- **Sakit**
- **Alpha**

Pemilihan satu baris pada tabel juga digunakan sebagai dasar untuk proses **Update** dan **Hapus**.

---

# 8. Pencarian Data

Aplikasi menyediakan fitur pencarian langsung melalui `TextBox`:

```text
Cari (NRP / Nama / Mata Kuliah)
```

Pencarian dilakukan berdasarkan:

- NRP

  <img width="642" height="128" alt="image" src="https://github.com/user-attachments/assets/c1676f63-9ea2-4d85-8da7-4aadad1ef4e2" />
  
- Nama

  <img width="645" height="106" alt="image" src="https://github.com/user-attachments/assets/9c0a7cea-807c-4eaa-b5aa-ad8b0b0456e1" />

- Mata Kuliah

  <img width="642" height="169" alt="image" src="https://github.com/user-attachments/assets/a95f82ef-6a58-4b45-8be2-fc5ce9171e94" />

Ketika isi kotak pencarian berubah, event `TextChanged` dijalankan dan tabel langsung diperbarui.

Contoh:

```text
Input:
067/Bagus/Pemrogramman
```

Maka tabel hanya menampilkan data yang berhubungan dengan keyword tersebut.

---

# 9. Filter Status

<img width="641" height="372" alt="image" src="https://github.com/user-attachments/assets/6dec59e1-75d4-403c-94ad-7f41d09ec07b" />

Selain pencarian, tersedia filter berdasarkan status.

Pilihan filter:

```text
Semua Status
Hadir
Izin
Sakit
Alpha
```

<img width="648" height="148" alt="image" src="https://github.com/user-attachments/assets/708d8add-a0b4-4f02-baaa-d14195f26eff" />

Ketika pilihan filter berubah, event `SelectionChanged` akan memanggil proses pemuatan ulang data.

Contoh:

```text
Filter = Alpha
```

Maka hanya data dengan status:

```text
Alpha
```

yang ditampilkan.

---

# 10. Ringkasan Data

<img width="350" height="29" alt="image" src="https://github.com/user-attachments/assets/5855c42e-4578-4cbc-8edc-f5196876842f" />

Pada bagian bawah tabel terdapat ringkasan data.

Formatnya:

```text
Total: ... data | Hadir: ... | Izin: ... | Sakit: ... | Alpha: ... | Kehadiran: ...%
```

Persentase kehadiran dihitung dengan rumus:

```text
Persentase Kehadiran =
Jumlah Hadir / Total Data × 100%
```

Ringkasan ini mengikuti data yang sedang ditampilkan.

Artinya, ketika pengguna menggunakan pencarian atau filter status, jumlah data pada ringkasan juga ikut berubah.

---

# 11. `Models/AttendanceRecord.cs`

File ini merupakan model yang merepresentasikan satu data absensi.

Property yang digunakan:

```text
Id
Nrp
Nama
MataKuliah
Tanggal
Status
Keterangan
```

Model ini digunakan sebagai objek untuk memindahkan data antara form, `DataGrid`, dan database.

Terdapat juga property:

```text
TanggalText
```

yang digunakan untuk menampilkan tanggal dengan format:

```text
dd-MM-yyyy
```

---

# 12. `Data/DatabaseHelper.cs`

File `DatabaseHelper.cs` menangani seluruh komunikasi antara aplikasi dan database SQLite.

Tanggung jawab utamanya:

- Membuat database.
- Membuat tabel.
- Membuat data dummy.
- Membaca data.
- Menambahkan data.
- Mengubah data.
- Menghapus data.
- Mengecek data duplikat.

Dengan pemisahan ini, proses database tidak diletakkan langsung di dalam kode tampilan.

---

# 13. Database SQLite

Nama database:

```text
attendance.db
```

Database dibuat secara otomatis pada folder output aplikasi.

Tabel yang digunakan:

```text
Attendance
```

Struktur tabel:

| Field | Tipe | Keterangan |
|---|---|---|
| `Id` | INTEGER | Primary key dan auto increment |
| `Nrp` | TEXT | NRP mahasiswa |
| `Nama` | TEXT | Nama mahasiswa |
| `MataKuliah` | TEXT | Mata kuliah |
| `Tanggal` | TEXT | Tanggal absensi |
| `Status` | TEXT | Status kehadiran |
| `Keterangan` | TEXT | Keterangan tambahan |

---

# 14. Inisialisasi Database

Saat window selesai dimuat, method:

```text
Window_Loaded
```

memanggil:

```csharp
DatabaseHelper.Initialize();
```

Proses tersebut:

1. Membuka koneksi SQLite.
2. Membuat tabel `Attendance` jika belum tersedia.
3. Mengecek jumlah data.
4. Jika database masih kosong, memasukkan 20 data dummy.
5. Mengisi daftar mata kuliah.
6. Menampilkan data pada `DataGrid`.

Dengan mekanisme ini, aplikasi dapat langsung digunakan untuk pengujian tanpa perlu memasukkan seluruh data secara manual.

---

# 15. Data Dummy

Saat database masih kosong, aplikasi membuat 20 data dummy.

Data dummy memiliki variasi status:

```text
Hadir
Izin
Sakit
Alpha
```

Data tersebut digunakan untuk mempermudah demonstrasi:

- Tampilan tabel.
- Filter.
- Pencarian.
- Perhitungan ringkasan.
- Update.
- Delete.

---

# 16. Proses CRUD

Aplikasi menerapkan operasi CRUD.

## Create

Digunakan ketika pengguna menekan:

```text
Simpan
```

Data akan dimasukkan ke tabel menggunakan:

```sql
INSERT INTO Attendance
```

---

## Read

Data dibaca menggunakan method:

```text
DatabaseHelper.GetAll()
```

Method tersebut mendukung:

- Pencarian.
- Filter status.
- Pengurutan berdasarkan tanggal.

Hasilnya kemudian ditampilkan pada `DataGrid`.

---

## Update

Ketika pengguna memilih baris kemudian mengubah data dan menekan:

```text
Update
```

aplikasi menjalankan:

```sql
UPDATE Attendance
```

---

## Delete

Ketika pengguna memilih data dan menekan:

```text
Hapus
```

setelah konfirmasi, aplikasi menjalankan:

```sql
DELETE FROM Attendance
```

---

# 17. Validasi Input

Validasi dilakukan sebelum proses `INSERT` maupun `UPDATE`.

## Validasi NRP

<img width="332" height="119" alt="image" src="https://github.com/user-attachments/assets/bdfbdcb6-6202-4d10-b7b9-4020139023bf" />

NRP:

- Tidak boleh kosong.
- Harus berupa angka.
- Harus terdiri dari 8–12 digit.

Contoh valid:

```text
5025241067
```

Contoh tidak valid:

```text
ABC123
```

---

## Validasi Nama

<img width="346" height="146" alt="image" src="https://github.com/user-attachments/assets/a9cf3f96-be9e-4ddb-8b72-001d2d467c01" />

Nama:

- Tidak boleh kosong.
- Minimal 3 karakter.

---

## Validasi Mata Kuliah

<img width="380" height="141" alt="image" src="https://github.com/user-attachments/assets/c9277b6a-08ea-4501-bafd-d85f87b55252" />

Pengguna harus memilih salah satu mata kuliah.

---

## Validasi Tanggal

<img width="406" height="222" alt="image" src="https://github.com/user-attachments/assets/2603f1f5-177e-44d5-999f-cc931dc1dc09" />

Tanggal harus dipilih dan tidak boleh melebihi tanggal hari ini.

---

## Validasi Keterangan

<img width="270" height="356" alt="image" src="https://github.com/user-attachments/assets/36a5a772-1452-4e0e-a124-3f9f0e79b163" />

Jika status:

```text
Izin
```

atau:

```text
Sakit
```

maka keterangan wajib diisi.

---

## Validasi Data Duplikat

<img width="923" height="268" alt="image" src="https://github.com/user-attachments/assets/dd40f79c-36b2-4fc5-ae1c-a4dbc6d2e649" />

Aplikasi juga melakukan pengecekan kombinasi:

```text
NRP + Mata Kuliah + Tanggal
```

Jika kombinasi tersebut sudah terdapat di database, data baru tidak akan disimpan.

Pada proses Update, ID data yang sedang diubah dikecualikan dari pemeriksaan duplikat sehingga data dapat diperbarui tanpa dianggap sebagai duplikat dirinya sendiri.

---

# 18. Event yang Digunakan

Beberapa event utama yang digunakan pada aplikasi:

| Event | Penggunaan |
|---|---|
| `Loaded` | Inisialisasi database dan pemuatan data awal |
| `Click` | Tombol Simpan, Update, Reset, dan Hapus |
| `SelectionChanged` | Pemilihan data dan perubahan filter |
| `TextChanged` | Pencarian data secara langsung |

Event-event tersebut menghubungkan interaksi pengguna dengan logic aplikasi.

---

# 19. Alur Kerja Aplikasi

Secara umum alur aplikasi:

```text
Aplikasi Dibuka
      ↓
Window Loaded
      ↓
Inisialisasi SQLite
      ↓
Cek Data
      ↓
Jika Kosong → Isi 20 Data Dummy
      ↓
Tampilkan Data
      ↓
┌───────────────┬────────────────┐
│ Form Absensi  │ Data Absensi   │
└───────────────┴────────────────┘
      ↓
Pengguna melakukan aksi
      ↓
Simpan / Update / Hapus
      ↓
Validasi
      ↓
Database SQLite
      ↓
DataGrid diperbarui
      ↓
Ringkasan diperbarui
```

---

# 20. Dokumentasi Tampilan Aplikasi

## 20.1 Tampilan Utama

<img width="959" height="605" alt="image" src="https://github.com/user-attachments/assets/a1235083-c0d3-461a-bf69-57604e6d8102" />

## 20.2 Form Tambah Data

<img width="270" height="509" alt="image" src="https://github.com/user-attachments/assets/438514eb-40eb-4370-b004-2d55c35fc72b" />

## 20.3 Data Berhasil Ditambahkan

<img width="635" height="88" alt="image" src="https://github.com/user-attachments/assets/8fb93fbd-a830-4a66-9ee4-482b625e775a" />

## 20.4 Proses Update

<img width="267" height="513" alt="image" src="https://github.com/user-attachments/assets/5b019345-ef56-4875-a774-a2b29b59cbef" />

<img width="634" height="91" alt="image" src="https://github.com/user-attachments/assets/9dff5920-10b9-41db-9b39-a3bf639b71fb" />

## 20.5 Proses Hapus

<img width="266" height="533" alt="image" src="https://github.com/user-attachments/assets/1eb6088e-8597-4969-a3a1-87e18e2c4602" />

<img width="635" height="122" alt="image" src="https://github.com/user-attachments/assets/e7485922-9e15-4c28-b2b0-b659fe5b79bf" />

## 20.6 Pencarian

<img width="650" height="98" alt="image" src="https://github.com/user-attachments/assets/7f2ba595-1a37-4715-b7f9-3c0aae5e72af" />

<img width="641" height="98" alt="image" src="https://github.com/user-attachments/assets/97f44926-dea7-4448-8b26-f3e0fc3e3c45" />

<img width="640" height="168" alt="image" src="https://github.com/user-attachments/assets/34dc9fb1-4b48-44b1-be47-91ca7458ec4f" />

## 20.7 Filter Status

<img width="640" height="143" alt="image" src="https://github.com/user-attachments/assets/1d779ea2-f900-40fa-8857-38c14fe74448" />

<img width="640" height="124" alt="image" src="https://github.com/user-attachments/assets/261c1218-7f90-4697-850f-7c4f3e92db8a" />

## Kesimpulan

Student Attendance App merupakan aplikasi pengelolaan absensi mahasiswa berbasis desktop yang menerapkan konsep:

- WPF dan XAML untuk antarmuka.
- C# untuk logic aplikasi.
- SQLite untuk penyimpanan data.
- Model `AttendanceRecord` untuk merepresentasikan data.
- `DatabaseHelper` untuk pengelolaan database.
- CRUD untuk pengelolaan data absensi.
- Validasi input untuk menjaga data yang masuk.
- Search dan filter untuk mempermudah pencarian data.
- Ringkasan data untuk memberikan informasi jumlah dan persentase kehadiran.

---

## Disclaimer

> **Data yang digunakan dalam aplikasi ini merupakan data dummy (data simulasi) yang dibuat semata-mata untuk keperluan pengembangan, pengujian, dan demonstrasi aplikasi. Data tersebut tidak merepresentasikan data mahasiswa, dosen, maupun pihak institusi yang sebenarnya dan tidak dimaksudkan untuk digunakan sebagai data resmi atau untuk keperluan administratif.**

---
