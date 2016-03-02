﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;
using MyAIAsisstent.Properties;

namespace MyAIAsisstent.Controls
{
    public partial class ReminderControl : UserControl
    {
        [Category("Appearance")]
        public Image Icon { get; set; }

        [Category("Appearance")]
        public string Message
        {
            get { return MessageLabel.Text; }
            set { MessageLabel.Text = value; }
        }

        private DateTime remindTime;
        [Browsable(false)]
        public DateTime RemindTime
        {
            get { return remindTime; }
            set
            {
                remindTime= value;
                DateTime temp = DateTime.Now;
                if (remindTime.Date == temp.Date)
                {
                    day = "Today  ";
                }
                else if (remindTime.AddDays(1).Date == temp.Date)
                    day = "Tomorrow  ";
                else day = remindTime.DayOfWeek.ToString() + "  ";
                TimeLabel.Text = day + remindTime.ToShortDateString() + "  At  " + remindTime.ToShortTimeString();
            }
        }

        private bool remindFinish = false;
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool RemindFinish
        {
            get
            {
                return remindFinish;
            }
            set
            {
                remindFinish = value;
                CheckFinish();
            }
        }

        [Category("Behavior")]
        //[ReadOnly(true)]
        //[Browsable(false)]
        public new MaterialForm ParentForm
        {
            //get { return (Main)(Program._main); }
            //set { ParentForm = (Main)(Program._main); }
            get; set;
        }

        private ReminderMouseState lastState = ReminderMouseState.Default;
        [ReadOnly(true)]
        [Browsable(false)]
        public ReminderMouseState LastState
        {
            get
            {
                return lastState;
            }
            set
            {
                if (value == ReminderMouseState.LostFocus)
                {
                    lastState = ReminderMouseState.Hover;
                    this.OnMouseLeave(new EventArgs());
                    lastReminderClick = null;
                    return;
                }
                if (value == lastState || lastState == ReminderMouseState.Clicked) return;
                switch (value)
                {
                    case ReminderMouseState.Default:
                        {
                            lastState = ReminderMouseState.Hover;
                            this.OnMouseLeave(new EventArgs());
                            break;
                        }
                    case ReminderMouseState.Hover:
                        {
                            lastState = ReminderMouseState.Default;
                            this.OnMouseEnter(new EventArgs());
                            break;
                        }

                }
            }
        }

        private string day;
        public static ReminderControl lastReminderClick;

        public ReminderControl()
        {
            InitializeComponent();
            if (remindFinish) pictureBox1.Show();
        }

        private void ReminderControl_Load(object sender, EventArgs e)
        {
            CheckFinish();
            if (!DesignMode)
            {
                base.BackColor = ((Main)ParentForm).NoteLabelColor(0);
                UpdateSkin(((Main)Program._main), new EventArgs());
                ParentForm.BackColorChanged += UpdateSkin;
            }
        }

        private void UpdateSkin(object sender, EventArgs e)
        {
            MessageLabel.ForeColor = ParentForm.SkinManager.GetPrimaryTextColor();
            TimeLabel.ForeColor = ParentForm.SkinManager.GetPrimaryTextColor();
            if (lastState == ReminderMouseState.Default)
            {
                base.BackColor = ((Main)ParentForm).NoteLabelColor(0);
            }
            else if (lastState == ReminderMouseState.Clicked)
            {
                base.BackColor = ((Main)ParentForm).NoteLabelColor(Main.NoteLabelStatus.Clicked);
            }
        }

        private void CheckFinish()
        {
            if (remindFinish)
            {
                MessageLabel.MaximumSize = new Size(this.Size.Width - 35 - 5, 25);
                MessageLabel.Location = new Point(35, 5);
                TimeLabel.Location = new Point(35, MessageLabel.Location.Y + MessageLabel.Size.Height + 5);
            }
            else
            {
                MessageLabel.MaximumSize = new Size(this.Size.Width - 10, 25);
                MessageLabel.Location = new Point(3, 5);
                TimeLabel.Location = new Point(3, MessageLabel.Location.Y + MessageLabel.Size.Height + 5);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (lastReminderClick != this )
            {
                if (lastReminderClick != null)
                lastReminderClick.lastState = ReminderMouseState.LostFocus;
                lastReminderClick = this;
                if (lastState != ReminderMouseState.Clicked)
                {
                    if (ParentForm.SkinManager.Theme == MaterialSkinManager.Themes.DARK)
                    {
                        base.BackColor = Color.FromArgb(110, 110, 110);
                    }
                    else base.BackColor = Color.FromArgb(170, 170, 170);
                    lastState = ReminderMouseState.Clicked;
                }
            }
        }

        private void MessageLabel_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void TimeLabel_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (lastState == ReminderMouseState.Default)
            {
                if (ParentForm.SkinManager.Theme == MaterialSkinManager.Themes.DARK)
                {
                    base.BackColor = ParentForm.SkinManager.GetFlatButtonHoverBackgroundColor();
                }
                else base.BackColor = Color.FromArgb(205, 205, 205);
                this.Cursor = Main.HandCursor;
                lastState = ReminderMouseState.Hover;
            }
        }

        private void MessageLabel_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void TimeLabel_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (lastState == ReminderMouseState.Hover)
            {
                if (ParentForm.SkinManager.Theme == MaterialSkinManager.Themes.DARK)
                {
                    base.BackColor = Color.FromArgb(60, 60, 60);
                }
                else base.BackColor = (ParentForm).SkinManager.GetFlatButtonHoverBackgroundColor();
                this.Cursor = Cursors.Default;
                lastState = ReminderMouseState.Default;
            }
        }

        private void MessageLabel_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void TimeLabel_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        public enum ReminderMouseState
        {
            Hover,
            Default,
            Clicked,
            LostFocus
        }
    }
}
