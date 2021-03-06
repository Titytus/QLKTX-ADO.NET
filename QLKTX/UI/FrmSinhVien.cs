﻿using QLKTX.BS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX.UI
{
    public partial class FrmSinhVien : Form
    {
        bool isExpended = false;
        string error = "";
        string strAvt = "";

        public FrmSinhVien()
        {
            InitializeComponent();
            btnSua.Visible = false;
        }

        public FrmSinhVien(string MSSV)
        {
            InitializeComponent();
            var dt = FrmMain.bS_Layer.Select(ref error, BS_layer.TableName.SinhVien, EnumConst.SinhVien.MSSV, MSSV);
            if (error != "")
            {
                MessageBox.Show("Đã xảy ra lối trong quá trình lưu dữ liệu. \n" + error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dt != null)
            {
                txtMSSV.Text = dt.Rows[0]["MSSV"].ToString();
                txtHoTen.Text = dt.Rows[0]["HoTen"].ToString();
                txtCMND.Text = dt.Rows[0]["CMND"].ToString();
                ckbNu.Checked = (bool) dt.Rows[0]["Phai"];
                cmbDienSV.Text = dt.Rows[0]["DienSV"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();
                txtNgSinh.Text = ((DateTime)dt.Rows[0]["NgSinh"]).ToString("yyyy-MM-dd");
                txtQueQuan.Text = dt.Rows[0]["QueQuan"].ToString();
                strAvt = dt.Rows[0]["AnhChanDung"].ToString();
                txtSDT.Text = dt.Rows[0]["SDT"].ToString();
                txtBHYT.Text = dt.Rows[0]["BHYT"].ToString();
                cmbMaLop.Text = dt.Rows[0]["MaLop"].ToString();
                try 
                {
                    if (strAvt != "")
                        picAvt.BackgroundImage = Image.FromFile(strAvt);
                }
                catch { }
            }
            pnContainer.Enabled = false;
        }

        private void btnMoRong_Click(object sender, EventArgs e)
        {
            isExpended = !isExpended;
            if (isExpended)
            {
                pnMoRong.Visible = true;
                this.Width = 1050;
            }
            else
            {
                pnMoRong.Visible = false;
                this.Width = 735;
            }
        }
        private bool CheckInput()
        {
            if (txtMSSV.Text == ""
                || cmbMaLop.Text == ""
                || txtHoTen.Text == ""
                || txtCMND.Text == ""
                || txtSDT.Text == ""
                || cmbDienSV.Text == "")
                return false;
            return true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (CheckInput() == false)
            {
                MessageBox.Show("Xin nhập đầy đủ thông tin cho sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dt = FrmMain.bS_Layer.Select(ref error, BS_layer.TableName.SinhVien, EnumConst.SinhVien.MSSV, txtMSSV.Text.Trim());
            bool bNu = (ckbNu.Checked) ? true : false;
            bool result = false;
            if (dt.Rows.Count > 0)
            {
                result = FrmMain.bS_Layer.Update(
                    txtMSSV.Text.Trim(),
                    cmbMaLop.Text.Trim(),
                    cmbDienSV.Text.Trim(),
                    txtHoTen.Text.Trim(),
                    Phai: bNu,
                    txtNgSinh.Text,
                    txtCMND.Text,
                    txtEmail.Text,
                    txtSDT.Text,
                    txtBHYT.Text,
                    txtQueQuan.Text,
                    strAvt,
                    ref error);
            }   
            else
            {
                result = FrmMain.bS_Layer.Insert(
                    txtMSSV.Text.Trim(),
                    cmbMaLop.Text.Trim(),
                    cmbDienSV.Text.Trim(),
                    txtHoTen.Text.Trim(),
                    Phai: bNu,
                    txtNgSinh.Text,
                    txtCMND.Text,
                    txtEmail.Text,
                    txtSDT.Text,
                    txtBHYT.Text,
                    txtQueQuan.Text,
                    strAvt.Trim(),
                    ref error);
            }
            if (result)
            {
                MessageBox.Show("Đã lưu xong!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }    
            else
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "Image (*.png)|*.png|Image (*.jpg)|*.jpg|All files (*.*)|*.*",
                Title = "Chọn ảnh chân dung"
            };
            var re = fileDialog.ShowDialog();
            if (re == DialogResult.OK)
            {
                strAvt = fileDialog.FileName;
                picAvt.BackgroundImage = Image.FromStream(fileDialog.OpenFile());
            }    
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            pnContainer.Enabled = true;
        }
    }
}
