// Zaitun Time Series 
// http://www.zaitunsoftware.com/
// http://code.google.com/p/zaitun-time-series/
//
// Copyright ©  2008-2009, Zaitun Time Series Developer Team and individual contributors
//
// Core Programmer: Rizal Zaini Ahmad Fathony (rizalzaf@gmail.com)
// Programmer: Suryono Hadi Wibowo, Khaerul Anas, Almaratul Sholihah, Muhamad Fuad Hasan
//
// This is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License as
// published by the Free Software Foundation; either version 3 of
// the License, or (at your option) any later version.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with this software; if not, write to the Free
// Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301 USA, or see the FSF site: http://www.fsf.org.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using zaitun.GUI;
using zaitun.Data;


namespace zaitun.ExponentialSmoothing
{
    public partial class ESSelectResultView : Form
    {

        private int initialModel;

        public ESSelectResultView()
        {
            InitializeComponent();

            this.Text = "Exponential Smoothing Select Result View";            
        }

        public void SetInitialModel(int initialModel)
        {
            this.initialModel = initialModel;
            if (this.initialModel == 1 || this.initialModel == 2)
            {
                this.trendCheckBox.Enabled = false;
                this.SeasonalCheckBox.Enabled = false;

                this.trendCheckBox.Checked = false;
                this.SeasonalCheckBox.Checked = false;
            }

            if (this.initialModel == 3)
            {
                this.SeasonalCheckBox.Enabled = false;
                this.SeasonalCheckBox.Checked = false;
            }
        }

        private void forecastedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool checkedCheck=((CheckBox)sender).Checked;
            this.forecastingStepTextBox.Enabled = checkedCheck;
            this.actualForecastedGraphCheckBox.Enabled = checkedCheck;
        }

        public bool IsEsModelSummaryChecked
        {
            get { return this.modelSummaryCheckBox.Checked; }
            set { this.modelSummaryCheckBox.Checked = value; }
        }

        public bool IsExponentialSmoothingDataGridChecked
        {
            get { return this.exponentialSmoothingCheckBox.Checked; }
            set { this.exponentialSmoothingCheckBox.Checked = value; }
        }

        public bool IsSmoothingChecked
        {
            get { return this.smoothingCheckBox.Checked; }
            set { this.smoothingCheckBox.Checked = value; }
        }

        public bool IsTrendChecked
        {
            get { return this.trendCheckBox.Checked; }
            set { this.trendCheckBox.Checked = value; }
        }

        public bool IsSeasonalChecked
        {
            get { return this.SeasonalCheckBox.Checked; }
            set { this.SeasonalCheckBox.Checked = value; }
        }

        public bool IsForecastedDataGridChecked
        {
            get { return this.forecastedCheckBox.Checked; }
            set { this.forecastedCheckBox.Checked = value; }
        }

        public bool IsActualAndPredictedGraphChecked
        {
            get { return this.actualPredictedGraphCheckBox.Checked; }
            set { this.actualPredictedGraphCheckBox.Checked = value; }
        }

        public bool IsActualAndSmoothedGraphChecked
        {
            get { return this.actualSmoothedGraphCheckBox.Checked; }
            set { this.actualSmoothedGraphCheckBox.Checked = value; }
        }
              
        public bool IsActualAndForecastedGraphChecked
        {
            get { return this.actualForecastedGraphCheckBox.Checked; }
            set { this.actualForecastedGraphCheckBox.Checked = value; }
        }

        public bool IsActualVsPredictedGraphChecked
        {
            get { return this.actualVsPredictedGraphCheckBox.Checked; }
            set { this.actualVsPredictedGraphCheckBox.Checked = value; }
        }

        public bool IsResidualGraphChecked
        {
            get { return this.residualGraphCheckBox.Checked; }
            set { this.residualGraphCheckBox.Checked = value; }
        }

        public bool IsResidualVsActualGraphChecked
        {
            get { return this.residualVsActualGraphCheckBox.Checked; }
            set { this.residualVsActualGraphCheckBox.Checked = value; }
        }

        public bool IsResidualVsPredictedGraphChecked
        {
            get { return this.residualVsPredictedCheckBox.Checked; }
            set { this.residualVsPredictedCheckBox.Checked = value; }
        }

        public int ForecastingStep
        {
            get
            {
                int result;
                try { result = int.Parse(forecastingStepTextBox.Text); }
                catch { result = 1; }
                return result;
            }
            set
            {
                if (value > 0)
                {
                    this.forecastingStepTextBox.Text = value.ToString();
                }
            }
        }

        private void exponentialSmoothingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.exponentialSmoothingCheckBox.Checked)
            {
                this.smoothingCheckBox.Enabled = true;
                this.actualPredictedAndResidualCheckBox.Checked = true;

                if (initialModel == 1 || this.initialModel == 2)
                {
                    this.trendCheckBox.Enabled = false;
                    this.SeasonalCheckBox.Enabled = false;
                }

                if (initialModel == 3)
                {
                    this.trendCheckBox.Enabled = true;
                    this.SeasonalCheckBox.Enabled = false;
                }

                if (this.initialModel == 4)
                {
                    this.trendCheckBox.Enabled = true;
                    this.SeasonalCheckBox.Enabled = true;
                }
            }
            else
            {
                this.smoothingCheckBox.Enabled = false;
                this.trendCheckBox.Enabled = false;
                this.SeasonalCheckBox.Enabled = false;
                this.actualPredictedAndResidualCheckBox.Checked = false;
            }
        }
    }
}