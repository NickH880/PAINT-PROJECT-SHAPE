using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SimplePaint
{
    public partial class PaintForm : Form
    {
        private ShapeType selectedShape; //Holds the shape type
        private Color shapeColor = Color.Black; //Shape color
        private int shapeX = 50, shapeY = 50, shapeWidth = 100, shapeHeight = 100; //Shape forms
        private bool isDragging = false; //Wether shape is dragged or not
        private Point dragStart; //Starting point for drawing
        private Rectangle currentShape; //Current shape being drawn

        public PaintForm()
        {
            InitializeComponent(); 
        }

        private void PaintForm_Load(object sender, EventArgs e)
        {
            shapeComboBox.Items.Add("Rectangle"); //Add shape options 
            shapeComboBox.Items.Add("Circle");
            shapeComboBox.SelectedIndex = 0; //Set selection to Rectangle

            ResetDefaults(); //Reset values
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput()) //Make sure input is accurate
            {
                CreateShape(); //Shape based on input
                this.Invalidate(); //Request a redraw
            }
        }

        private void PaintForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; //For graphics
            Brush brush = new SolidBrush(shapeColor); //Brush and color
            if (selectedShape == ShapeType.Rectangle) //Draw shape
                g.FillRectangle(brush, currentShape);
            else if (selectedShape == ShapeType.Circle)
                g.FillEllipse(brush, currentShape);
        }

        private void CreateShape()
        {
            selectedShape = (ShapeType)shapeComboBox.SelectedIndex; //Select shape type
            currentShape = new Rectangle(shapeX, shapeY, shapeWidth, shapeHeight); //Shape
        }

        private bool ValidateInput()
        {
            try
            {
                shapeX = int.Parse(xTextBox.Text); //For width and height
                shapeY = int.Parse(yTextBox.Text);
                shapeWidth = int.Parse(widthTextBox.Text);
                shapeHeight = int.Parse(heightTextBox.Text);

                
                if (shapeWidth <= 0 || shapeHeight <= 0) //For positive width and height
                {
                    MessageBox.Show("Width and Height must be positive");
                    return false;
                }
                return true; //Input is valid
            }
            catch
            {
                MessageBox.Show("Enter valid numbers"); //Handle parsing errors
                return false;
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetDefaults(); //For reset on input fields and shape settings
        }

        private void ResetDefaults()
        {
            xTextBox.Text = "50"; //Set X position
            yTextBox.Text = "50"; //Set Y position
            widthTextBox.Text = "100"; //Set width
            heightTextBox.Text = "100"; //Set height
            shapeColor = Color.Black; //Reset shape color 
            currentShape = new Rectangle(shapeX, shapeY, shapeWidth, shapeHeight); // Recreate current shape
            this.Invalidate(); // Request a redraw
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK) //Show color 
                shapeColor = colorDialog.Color; //Shape color
        }

        private void PaintForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentShape.Contains(e.Location)) //Mouse and Shape
            {
                isDragging = true; //For dragging
                dragStart = e.Location; //Starting position
            }
        }

        private void PaintForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging) //Update shape position
            {
                int offsetX = e.X - dragStart.X; //X offset
                int offsetY = e.Y - dragStart.Y; //Y offset

                shapeX += offsetX; //Shape X position
                shapeY += offsetY; //Shape Y position
                currentShape = new Rectangle(shapeX, shapeY, shapeWidth, shapeHeight); // Update shape rectangle

                this.Invalidate(); //Redraw
                dragStart = e.Location; //Update position for move
            }
        }

        private void PaintForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false; //Stop dragging
        }
    }

    enum ShapeType //Shape
    {
        Rectangle,
        Circle
    }
}
