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

`MainWindow.xaml` merupakan bagian yang mendefinisikan seluruh tampilan aplikasi.

Secara umum halaman dibagi menjadi dua bagian:

1. **Form Input Absensi**
2. **Data Absensi**

## 5.1 Form Input Absensi

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
- Nama
- Mata Kuliah

Ketika isi kotak pencarian berubah, event `TextChanged` dijalankan dan tabel langsung diperbarui.

Contoh:

```text
Input:
5025241001
```

Maka tabel hanya menampilkan data yang berhubungan dengan keyword tersebut.

---

# 9. Filter Status

Selain pencarian, tersedia filter berdasarkan status.

Pilihan filter:

```text
Semua Status
Hadir
Izin
Sakit
Alpha
```

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

Nama:

- Tidak boleh kosong.
- Minimal 3 karakter.

---

## Validasi Mata Kuliah

Pengguna harus memilih salah satu mata kuliah.

---

## Validasi Tanggal

Tanggal harus dipilih dan tidak boleh melebihi tanggal hari ini.

---

## Validasi Status

Pengguna harus memilih salah satu status:

```text
Hadir
Izin
Sakit
Alpha
```

---

## Validasi Keterangan

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

Bagian ini dapat digunakan untuk memasukkan screenshot hasil aplikasi.

## 20.1 Tampilan Utama

**Screenshot:**

> Tambahkan screenshot tampilan utama aplikasi di sini.

Contoh yang dapat ditampilkan:

- Form input.
- DataGrid.
- Search.
- Filter status.
- Ringkasan data.

---

## 20.2 Form Tambah Data

**Screenshot:**

> Tambahkan screenshot form ketika pengguna mengisi data absensi baru.

Jelaskan field yang digunakan:

- NRP
- Nama
- Mata Kuliah
- Tanggal
- Status
- Keterangan

---

## 20.3 Data Berhasil Ditambahkan

**Screenshot:**

> Tambahkan screenshot setelah tombol `Simpan` berhasil digunakan.

Tunjukkan bahwa data baru muncul pada DataGrid dan jumlah data pada ringkasan berubah.

---

## 20.4 Proses Update

**Screenshot:**

> Tambahkan screenshot ketika salah satu baris dipilih dan datanya muncul kembali pada form.

Kemudian tambahkan screenshot setelah tombol `Update` ditekan.

---

## 20.5 Proses Hapus

**Screenshot:**

> Tambahkan screenshot dialog konfirmasi penghapusan data.

Kemudian tambahkan screenshot DataGrid setelah data berhasil dihapus.

---

## 20.6 Pencarian

**Screenshot:**

> Tambahkan screenshot ketika keyword dimasukkan pada kolom pencarian.

Tunjukkan bahwa data pada tabel berubah sesuai keyword.

---

## 20.7 Filter Status

**Screenshot:**

> Tambahkan screenshot ketika filter `Hadir`, `Izin`, `Sakit`, atau `Alpha` digunakan.

Tunjukkan bahwa DataGrid hanya menampilkan data dengan status yang dipilih.

---

# 21. Skenario Pengujian

Pengujian dilakukan untuk memastikan fungsi utama aplikasi berjalan sesuai kebutuhan.

## A. Pengujian Form dan Validasi

| No | Skenario Pengujian | Input / Aksi | Hasil yang Diharapkan |
|---|---|---|---|
| 1 | Form kosong | Langsung klik `Simpan` | Muncul pesan `NRP harus diisi!` |
| 2 | NRP kosong | Nama dan field lain diisi, NRP kosong | Data tidak disimpan dan muncul validasi NRP |
| 3 | NRP mengandung huruf | `ABC123456` | Muncul pesan NRP harus berupa angka 8–12 digit |
| 4 | NRP terlalu pendek | `1234567` | Data ditolak |
| 5 | NRP terlalu panjang | NRP lebih dari 12 digit | Data ditolak |
| 6 | NRP valid | NRP 8–12 digit angka | Validasi NRP dilewati |
| 7 | Nama kosong | NRP diisi, nama kosong | Muncul pesan nama harus diisi |
| 8 | Nama terlalu pendek | Nama `AB` | Muncul pesan nama minimal 3 karakter |
| 9 | Mata kuliah kosong | Tidak memilih mata kuliah | Muncul pesan pilih mata kuliah |
| 10 | Tanggal kosong | Tidak memilih tanggal | Muncul pesan pilih tanggal |
| 11 | Tanggal masa depan | Memilih tanggal setelah hari ini | Muncul pesan tanggal tidak boleh melebihi hari ini |
| 12 | Status belum dipilih | Semua status tidak dipilih | Muncul pesan pilih status |
| 13 | Izin tanpa keterangan | Status `Izin`, keterangan kosong | Data ditolak |
| 14 | Sakit tanpa keterangan | Status `Sakit`, keterangan kosong | Data ditolak |
| 15 | Izin dengan keterangan | Status `Izin` + keterangan | Data dapat diproses |
| 16 | Sakit dengan keterangan | Status `Sakit` + keterangan | Data dapat diproses |
| 17 | Data valid | Semua field valid | Data berhasil disimpan |

---

## B. Pengujian CRUD

| No | Skenario Pengujian | Aksi | Hasil yang Diharapkan |
|---|---|---|---|
| 18 | Create | Isi form lalu klik `Simpan` | Data baru muncul di DataGrid |
| 19 | Create | Simpan data valid beberapa kali | Setiap data valid tersimpan |
| 20 | Duplicate | Masukkan NRP + mata kuliah + tanggal yang sama | Muncul pesan data sudah ada |
| 21 | Select Data | Klik salah satu baris DataGrid | Data masuk kembali ke form |
| 22 | Update | Ubah nama/status/keterangan lalu klik `Update` | Data pada tabel berubah |
| 23 | Update tanpa data | Tidak memilih baris lalu klik `Update` | Muncul pesan untuk memilih data |
| 24 | Delete | Pilih data lalu klik `Hapus` | Dialog konfirmasi muncul |
| 25 | Delete Cancel | Pada dialog pilih `No` | Data tetap ada |
| 26 | Delete Confirm | Pada dialog pilih `Yes` | Data dihapus dari DataGrid dan database |
| 27 | Reset | Klik `Reset` | Form kembali ke kondisi awal |

---

## C. Pengujian Pencarian dan Filter

| No | Skenario Pengujian | Aksi | Hasil yang Diharapkan |
|---|---|---|---|
| 28 | Search berdasarkan NRP | Masukkan NRP pada kotak pencarian | Data dengan NRP tersebut ditampilkan |
| 29 | Search berdasarkan nama | Masukkan sebagian nama | Data yang sesuai keyword ditampilkan |
| 30 | Search berdasarkan mata kuliah | Masukkan nama/sebagian mata kuliah | Data yang sesuai ditampilkan |
| 31 | Search tidak ditemukan | Masukkan keyword yang tidak ada | DataGrid kosong dan total menjadi 0 |
| 32 | Hapus keyword | Kosongkan kotak pencarian | Data kembali ditampilkan |
| 33 | Filter Hadir | Pilih `Hadir` | Hanya data Hadir yang tampil |
| 34 | Filter Izin | Pilih `Izin` | Hanya data Izin yang tampil |
| 35 | Filter Sakit | Pilih `Sakit` | Hanya data Sakit yang tampil |
| 36 | Filter Alpha | Pilih `Alpha` | Hanya data Alpha yang tampil |
| 37 | Semua Status | Pilih `Semua Status` | Seluruh data kembali tampil |
| 38 | Search + Filter | Gunakan pencarian sekaligus filter status | Data memenuhi kedua kondisi tersebut |

---

## D. Pengujian Ringkasan Data

| No | Skenario Pengujian | Aksi | Hasil yang Diharapkan |
|---|---|---|---|
| 39 | Data awal | Buka aplikasi | Ringkasan menampilkan total dan jumlah setiap status |
| 40 | Setelah tambah data | Simpan satu data | Total dan kategori status diperbarui |
| 41 | Setelah hapus data | Hapus satu data | Total dan kategori status berkurang |
| 42 | Setelah filter | Pilih salah satu status | Ringkasan mengikuti data yang sedang ditampilkan |
| 43 | Setelah pencarian | Masukkan keyword | Ringkasan mengikuti hasil pencarian |
| 44 | Data kosong | Gunakan keyword yang tidak ditemukan | Total menjadi 0 dan persentase kehadiran menjadi 0% |

---

# 22. Contoh Urutan Demonstrasi / Pengujian

Untuk demonstrasi aplikasi, pengujian dapat dilakukan dengan urutan berikut:

### Pengujian 1 — Menampilkan Data Awal

1. Jalankan aplikasi.
2. Pastikan DataGrid muncul.
3. Periksa data dummy.
4. Periksa ringkasan jumlah data.
5. Pastikan terdapat beberapa status berbeda.

**Screenshot yang dapat diambil:**

```text
Tampilan awal aplikasi + DataGrid + ringkasan
```

---

### Pengujian 2 — Menambahkan Data Hadir

1. Isi NRP.
2. Isi nama.
3. Pilih mata kuliah.
4. Pilih tanggal hari ini atau tanggal sebelumnya.
5. Pilih status `Hadir`.
6. Klik `Simpan`.
7. Periksa DataGrid.
8. Periksa ringkasan.

**Hasil yang diharapkan:**

Data baru muncul pada DataGrid dan jumlah data bertambah.

---

### Pengujian 3 — Validasi NRP

1. Kosongkan form.
2. Masukkan NRP berupa huruf.
3. Isi field lainnya.
4. Klik `Simpan`.

**Hasil yang diharapkan:**

Data tidak disimpan dan aplikasi menampilkan pesan validasi NRP.

---

### Pengujian 4 — Validasi Izin/Sakit

1. Pilih status `Izin`.
2. Kosongkan keterangan.
3. Klik `Simpan`.

**Hasil yang diharapkan:**

Aplikasi menolak data dan meminta keterangan.

Ulangi pengujian menggunakan status `Sakit`.

---

### Pengujian 5 — Mencegah Duplikat

1. Masukkan NRP yang sudah digunakan.
2. Pilih mata kuliah yang sama.
3. Gunakan tanggal yang sama.
4. Klik `Simpan`.

**Hasil yang diharapkan:**

Aplikasi mendeteksi kombinasi:

```text
NRP + Mata Kuliah + Tanggal
```

dan menolak data duplikat.

---

### Pengujian 6 — Update

1. Klik salah satu baris DataGrid.
2. Pastikan data masuk ke form.
3. Ubah status atau keterangan.
4. Klik `Update`.
5. Periksa kembali baris tersebut.

**Hasil yang diharapkan:**

Data lama berubah menjadi data baru.

---

### Pengujian 7 — Delete

1. Pilih satu baris.
2. Klik `Hapus`.
3. Periksa dialog konfirmasi.
4. Pilih `No`.

**Hasil:**

Data tetap ada.

5. Klik `Hapus` lagi.
6. Pilih `Yes`.

**Hasil:**

Data terhapus dari tabel.

---

### Pengujian 8 — Search

1. Klik kotak pencarian.
2. Masukkan sebagian NRP atau nama.
3. Amati DataGrid.

**Hasil yang diharapkan:**

DataGrid langsung menampilkan data yang sesuai dengan keyword.

---

### Pengujian 9 — Filter Status

1. Pilih `Alpha` pada filter status.
2. Amati DataGrid.
3. Ulangi dengan `Hadir`, `Izin`, dan `Sakit`.
4. Kembalikan filter ke `Semua Status`.

**Hasil yang diharapkan:**

DataGrid menampilkan data sesuai status yang dipilih.

---

### Pengujian 10 — Search + Filter

1. Masukkan keyword pada pencarian.
2. Pilih salah satu status.
3. Amati hasil DataGrid.

**Hasil yang diharapkan:**

Data yang tampil memenuhi kondisi pencarian sekaligus status yang dipilih.

---

# 23. Kesimpulan

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

Dokumentasi screenshot dapat ditambahkan pada bagian **Dokumentasi Tampilan Aplikasi** dan setiap screenshot dapat dikaitkan dengan skenario pengujian pada bagian **Skenario Pengujian**.
