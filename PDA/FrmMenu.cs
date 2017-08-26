using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PDA
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        private void btnDailyReport_Click(object sender, EventArgs e)
        {
            try
            {
                FrmDailyReport FrmDailyReport = new FrmDailyReport();
                FrmDailyReport.ShowDialog();
                FrmDailyReport.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOSGoodsIssu_Click(object sender, EventArgs e)
        {
            try
            {
                FrmOSGoodsIssu FrmOSGoodsIssu = new FrmOSGoodsIssu();
                FrmOSGoodsIssu.ShowDialog();
                FrmOSGoodsIssu.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOSGoodsRec_Click(object sender, EventArgs e)
        {
            try
            {
                FrmOSGoodsRec FrmOSGoodsRec = new FrmOSGoodsRec();
                FrmOSGoodsRec.ShowDialog();
                FrmOSGoodsRec.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWorkshooGR_Click(object sender, EventArgs e)
        {
            try
            {
                FrmWorkshooGR FrmWorkshooGR = new FrmWorkshooGR();
                FrmWorkshooGR.ShowDialog();
                FrmWorkshooGR.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRejectPackage_Click(object sender, EventArgs e)
        {
            try
            {
                FrmRejectPackage FrmRejectPackage = new FrmRejectPackage();
                FrmRejectPackage.ShowDialog();
                FrmRejectPackage.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearchPDA_Click(object sender, EventArgs e)
        {
            try
            {
                FrmSearchPDA FrmSearchPDA = new FrmSearchPDA();
                FrmSearchPDA.ShowDialog();
                FrmSearchPDA.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRecRFID_Click(object sender, EventArgs e)
        {
            try
            {
                FrmRecRFID FrmRecRFID = new FrmRecRFID();
                FrmRecRFID.ShowDialog();
                FrmRecRFID.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStopPDA_Click(object sender, EventArgs e)
        {
            try
            {
                FrmStopPDA FrmStopPDA = new FrmStopPDA();
                FrmStopPDA.ShowDialog();
                FrmStopPDA.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMapping_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMapping FrmMapping = new FrmMapping();
                FrmMapping.ShowDialog();
                FrmMapping.Dispose();
                GC.Collect();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOsMapping_Click(object sender, EventArgs e)
        {
            FrmOSMapping FrmMapping = new FrmOSMapping();
            FrmMapping.ShowDialog();
            FrmMapping.Dispose();
            GC.Collect();
        }


    }
}