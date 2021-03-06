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
using zaitun.Data;

namespace zaitun.ExponentialSmoothing
{
    public partial class SingleESGridSearch : Form
    {
        private SeriesVariable variable;
        private double selectedAlpha;
        List<SingleExponentialSmoothing> solutions = new List<SingleExponentialSmoothing>();

        public SingleESGridSearch()
        {
            InitializeComponent();
        }

        public void SetVariable(SeriesVariable variable)
        {
            this.variable = variable;

            this.Text = "Single ES Grid Search : " + variable.VariableName;
        }

        private void GridSearch()
        {
            double alphaStart = (double)this.alphaStartUpDown.Value;
            double alphaStop = (double)this.alphaStopUpDown.Value;
            double alphaIncrement = (double)this.alphaIncrementUpDown.Value;
            double currentAlpha = alphaStart;
            double solutionCount = double.Parse(solutionBox.Text);

            double maxSolutionsCriterion = 0;
            int maxSolutionIndex = -1;

            int increment = 0;
                                    
            while (currentAlpha <= alphaStop)
            {
                SingleExponentialSmoothing ses = new SingleExponentialSmoothing(variable, currentAlpha);                              
                
                double mse = ses.MSE;

                // Masukkan sejumlah solutionCount solusi pertama ke list
                if (solutions.Count < solutionCount)
                {
                    solutions.Add(ses);
                    if (ses.MSE > maxSolutionsCriterion)
                    {                            
                        maxSolutionsCriterion = ses.MSE;
                        maxSolutionIndex = solutions.Count - 1;
                    }
                }
                // list solusi sudah penuh
                else
                {
                    // jika lebih kecil dari MSE terbesar pada list, ganti solusi terbesar itu 
                    // dengan solusi ini
                    if (ses.MSE < maxSolutionsCriterion)
                    {
                        solutions[maxSolutionIndex] = ses;
                        maxSolutionIndex = this.FindMaximumCriterionIndex(solutions);
                        maxSolutionsCriterion = solutions[maxSolutionIndex].MSE;
                    }
                }                

                currentAlpha += alphaIncrement;
                ++increment;
            }

            //this.textBox2.Visible = true;
            //this.textBox2.Text = increment.ToString();

            // urutkan berdasarkan mse
            solutions.Sort(delegate(SingleExponentialSmoothing ses1, SingleExponentialSmoothing ses2) { return ses1.MSE.CompareTo(ses2.MSE); });

            this.resultGrid.RowCount = solutions.Count;
            // tampilkan ke grid
            for (int i = 0; i < solutions.Count; i++)
            {
                int row = i + 1;
                this.resultGrid.Rows[i].HeaderCell.Value = row.ToString();
                this.resultGrid[0, i].Value = solutions[i].Alpha.ToString("F3");
                this.resultGrid[1, i].Value = solutions[i].MAE.ToString("F5");
                this.resultGrid[2, i].Value = solutions[i].MSE.ToString("F5");
                this.resultGrid[3, i].Value = solutions[i].MPE.ToString("F5");
                this.resultGrid[4, i].Value = solutions[i].MAPE.ToString("F5");
            }            

        }

        private int FindMaximumCriterionIndex(List<SingleExponentialSmoothing> list)
        {
            int index = 0;
            double maxCriterion;            
                
            maxCriterion = list[0].MSE;
            for (int i = 1; i < list.Count; i++)                    
                if (list[i].MSE > maxCriterion)                        
                    index = i;

            return index;             
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (this.solutionBox.Text == "")
            {
                MessageBox.Show("Please insert the number of solution", "Single ES Grid Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
            else if (int.Parse(this.solutionBox.Text) > 100)
            {
                MessageBox.Show("Solution must lest than 100", "Single ES Grid Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.selectButton.Enabled = true;
                this.solutions.Clear();
                this.resultGrid.Rows.Clear();

                //DateTime start = new DateTime();
                //DateTime finish = new DateTime();
                //TimeSpan time = new TimeSpan();
                //timer1.Enabled = true;
                //start = DateTime.Now;
                //this.timer1.Enabled = true;

                this.GridSearch();
                //finish = DateTime.Now;
                //timer1.Enabled = false;
                //time = finish - start;
                //this.textBox1.Visible = true;
                //this.textBox1.Text = time.ToString();
            }
        }

        public double SelectedAlpha
        {
            get { return this.selectedAlpha; }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (this.resultGrid.SelectedRows.Count != 0)
                this.selectedAlpha = this.solutions[this.resultGrid.SelectedRows[0].Index].Alpha;
            else
                this.DialogResult = DialogResult.None;            
        }

        private void SingleESGridSearch_Load(object sender, EventArgs e)
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 500000000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 1;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this, "NOTE:\nClick search button to perform the grid search\nfor parameters and to display the Parameter Grid Search in Best Result.");
            toolTip1.SetToolTip(this.resultGrid, "BEST RESULT:\nThis grid display sets of parameters that resulted in the smallest mean squared error.");
            toolTip1.SetToolTip(this.selectButton, "Click here to continue and use the selected case");
            toolTip1.SetToolTip(this.cancelButton, "Click here to continue whithout select a case");
            toolTip1.SetToolTip(this.label1, "ALPHA:\nSmoothing constant for level\n\nRANGE:\nAlpha should fall into the interval between 0 (zero) and 1");
            toolTip1.SetToolTip(this.alphaStartUpDown, "Type the start value of parameter alpha");
            toolTip1.SetToolTip(this.alphaIncrementUpDown, "Type the value to increment parameter alpha");
            toolTip1.SetToolTip(this.alphaStopUpDown, "Type the end value of alpha");
            toolTip1.SetToolTip(this.solutionBox, "Type here of how many parameter values will be display");
            toolTip1.SetToolTip(this.searchButton, "Click here to perform the grid search for parameters and to display the Parameter Gird Search in Best Result.");
        }
    }
}