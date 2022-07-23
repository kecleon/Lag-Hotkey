﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LagKey.Properties;

namespace LagKey
{
	public partial class KeyForm : Form
	{
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		private Keys Key = Keys.F4;
		private int Port = 2050;
		private bool Lagging = false;
		private Process NetSH;

		public KeyForm()
		{
			InitializeComponent();

			//setup netsh session
			SetupNetSH();
			
			this.KeyPreview = true;
			this.txtKey.KeyDown += TxtKeyOnKeyDown;
			this.Closing += OnClosing;

			//register default hotkey
			RegisterHotKey(this.Handle, 1, 0, (int)Key);
		}

		protected override void WndProc(ref Message m)
		{
			//handle hotkey callback
			if (m.Msg == 0x0312 && m.WParam.ToInt32() == 1)
			{
				ToggleLag();
			}

			base.WndProc(ref m);
		}

		private void OnClosing(object sender, CancelEventArgs e)
		{
			//remove hotkey and clear filters
			UnregisterHotKey(this.Handle, 1);
			NetSH.StandardInput.WriteLine("ipsec static delete all");
			NetSH.Kill();
		}

		private void TxtKeyOnKeyDown(object sender, KeyEventArgs e)
		{
			//disable hotkey, register new one
			UnregisterHotKey(this.Handle, 1);
			Key = e.KeyCode;
			txtKey.Text = GetDescription(Key);
			RegisterHotKey(this.Handle, 1, 0, (int)Key);
		}

		private void txtPort_TextChanged(object sender, EventArgs e)
		{
			//prevent users from typing invalid ports
			if (!int.TryParse(txtPort.Text, out var port))
			{
				txtPort.Text = Port.ToString();
				return;
			}

			//if user changes port while lagging, disable lag beforehand so it's not perma stuck filtered
			if (Lagging)
			{
				ToggleLag();
			}

			//prevent invalid ports...
			if (port > 65535)
			{
				port = 65535;
				txtPort.Text = "65535";
			}

			if (port != Port)
			{
				//reset filter to new port
				UpdatePolicyPort();
			}

			Port = port;
		}

		private void SetupNetSH()
		{
			//create netsh console, keep it open so toggling can write to existing session  
			var startInfo = new ProcessStartInfo
			{
				FileName = "netsh.exe",
				RedirectStandardInput = true,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				ErrorDialog = false,
			};

			NetSH = Process.Start(startInfo);
			NetSH.StandardInput.AutoFlush = true;
			UpdatePolicyPort();
		}

		private void UpdatePolicyPort()
		{
			//couldn't figure out how to update an existing filter via netsh set, nor did I find any information on how to do that online, so deleting and recreating is the answer
			NetSH.StandardInput.WriteLine("ipsec static delete all");
			NetSH.StandardInput.WriteLine("ipsec static add policy lag");
			NetSH.StandardInput.WriteLine("ipsec static add filterlist lag");
			NetSH.StandardInput.WriteLine("ipsec static add filter filterlist=lag srcaddr=any dstaddr=any protocol=tcp dstport=" + Port);
			NetSH.StandardInput.WriteLine("ipsec static add filteraction lag action=block");
			NetSH.StandardInput.WriteLine("ipsec static add rule name=lag policy=lag filterlist=lag filteraction=lag");
		}

		private void ToggleLag()
		{
			if (Lagging)
			{
				//disable

				Lagging = false;

				NetSH.StandardInput.WriteLine("ipsec static set policy lag assign=n");

				this.lblStatus.Text = "OFF";
				this.lblStatus.ForeColor = Color.Red;
				this.Icon = Resources.connected;
			}
			else
			{
				//enable

				Lagging = true;

				NetSH.StandardInput.WriteLine("ipsec static set policy lag assign=y");

				this.lblStatus.Text = "ON";
				this.lblStatus.ForeColor = Color.Lime;
				this.Icon = Resources.disconnected;
			}
		}

		//vk name (Oem5) to proper human friendly name
		public static string GetDescription(Keys key)
		{
			switch (key)
			{
				//letters
				case Keys.A:
				case Keys.B:
				case Keys.C:
				case Keys.D:
				case Keys.E:
				case Keys.F:
				case Keys.G:
				case Keys.H:
				case Keys.I:
				case Keys.J:
				case Keys.K:
				case Keys.L:
				case Keys.M:
				case Keys.N:
				case Keys.O:
				case Keys.P:
				case Keys.Q:
				case Keys.R:
				case Keys.S:
				case Keys.T:
				case Keys.U:
				case Keys.V:
				case Keys.W:
				case Keys.X:
				case Keys.Y:
				case Keys.Z:
					return Enum.GetName(typeof(Keys), key);

				//digits
				case Keys.D0:
					return "0";
				case Keys.NumPad0:
					return "Numpad 0";
				case Keys.D1:
					return "1";
				case Keys.NumPad1:
					return "Numpad 1";
				case Keys.D2:
					return "2";
				case Keys.NumPad2:
					return "Numpad 2";
				case Keys.D3:
					return "3";
				case Keys.NumPad3:
					return "Numpad 3";
				case Keys.D4:
					return "4";
				case Keys.NumPad4:
					return "Numpad 4";
				case Keys.D5:
					return "5";
				case Keys.NumPad5:
					return "Numpad 5";
				case Keys.D6:
					return "6";
				case Keys.NumPad6:
					return "Numpad 6";
				case Keys.D7:
					return "7";
				case Keys.NumPad7:
					return "Numpad 7";
				case Keys.D8:
					return "8";
				case Keys.NumPad8:
					return "Numpad 8";
				case Keys.D9:
					return "9";
				case Keys.NumPad9:
					return "Numpad 9";

				//punctuation
				case Keys.Add:
					return "Numpad +";
				case Keys.Subtract:
					return "Numpad -";
				case Keys.Divide:
					return "Numpad /";
				case Keys.Multiply:
					return "Numpad *";
				case Keys.Space:
					return "Spacebar";
				case Keys.Decimal:
					return "Numpad .";

				//function
				case Keys.F1:
				case Keys.F2:
				case Keys.F3:
				case Keys.F4:
				case Keys.F5:
				case Keys.F6:
				case Keys.F7:
				case Keys.F8:
				case Keys.F9:
				case Keys.F10:
				case Keys.F11:
				case Keys.F12:
				case Keys.F13:
				case Keys.F14:
				case Keys.F15:
				case Keys.F16:
				case Keys.F17:
				case Keys.F18:
				case Keys.F19:
				case Keys.F20:
				case Keys.F21:
				case Keys.F22:
				case Keys.F23:
				case Keys.F24:
					return key.ToString();

				//navigation
				case Keys.Up:
					return "Up Arrow";
				case Keys.Down:
					return "Down Arrow";
				case Keys.Left:
					return "Left Arrow";
				case Keys.Right:
					return "Right Arrow";
				case Keys.Prior:
					return "Page Up";
				case Keys.Next:
					return "Page Down";
				case Keys.Home:
					return "Home";
				case Keys.End:
					return "End";

				//control keys
				case Keys.Back:
					return "Backspace";
				case Keys.Tab:
					return "Tab";
				case Keys.Escape:
					return "Escape";
				case Keys.Enter:
					return "Enter";
				case Keys.Shift:
				case Keys.ShiftKey:
					return "Shift";
				case Keys.LShiftKey:
					return "Shift (Left)";
				case Keys.RShiftKey:
					return "Shift (Right)";
				case Keys.Control:
				case Keys.ControlKey:
					return "Control";
				case Keys.LControlKey:
					return "Control (Left)";
				case Keys.RControlKey:
					return "Control (Right)";
				case Keys.Menu:
				case Keys.Alt:
					return "Alt";
				case Keys.LMenu:
					return "Alt (Left)";
				case Keys.RMenu:
					return "Alt (Right)";
				case Keys.Pause:
					return "Pause";
				case Keys.CapsLock:
					return "Caps Lock";
				case Keys.NumLock:
					return "Num Lock";
				case Keys.Scroll:
					return "Scroll Lock";
				case Keys.PrintScreen:
					return "Print Screen";
				case Keys.Insert:
					return "Insert";
				case Keys.Delete:
					return "Delete";
				case Keys.Help:
					return "Help";
				case Keys.LWin:
					return "Windows (Left)";
				case Keys.RWin:
					return "Windows (Right)";
				case Keys.Apps:
					return "Context Menu";

				//browser keys
				case Keys.BrowserBack:
					return "Browser Back";
				case Keys.BrowserFavorites:
					return "Browser Favorites";
				case Keys.BrowserForward:
					return "Browser Forward";
				case Keys.BrowserHome:
					return "Browser Home";
				case Keys.BrowserRefresh:
					return "Browser Refresh";
				case Keys.BrowserSearch:
					return "Browser Search";
				case Keys.BrowserStop:
					return "Browser Stop";

				//media keys
				case Keys.VolumeDown:
					return "Volume Down";
				case Keys.VolumeMute:
					return "Volume Mute";
				case Keys.VolumeUp:
					return "Volume Up";
				case Keys.MediaNextTrack:
					return "Next Track";
				case Keys.Play:
				case Keys.MediaPlayPause:
					return "Play";
				case Keys.MediaPreviousTrack:
					return "Previous Track";
				case Keys.MediaStop:
					return "Stop";
				case Keys.SelectMedia:
					return "Select Media";

				//IME keys
				case Keys.HanjaMode:
				case Keys.JunjaMode:
				case Keys.HangulMode:
				case Keys.FinalMode: //duplicate values: Hanguel, Kana, Kanji  
				case Keys.IMEAccept:
				case Keys.IMEConvert: //duplicate: IMEAceept
				case Keys.IMEModeChange:
				case Keys.IMENonconvert:
					return null;

				//special keys
				case Keys.LaunchMail:
					return "Launch Mail";
				case Keys.LaunchApplication1:
					return "Launch Favorite Application 1";
				case Keys.LaunchApplication2:
					return "Launch Favorite Application 2";
				case Keys.Zoom:
					return "Zoom";

				//oem keys 
				case Keys.OemSemicolon: //oem1
					return ";";
				case Keys.OemQuestion: //oem2
					return "?";
				case Keys.Oemtilde: //oem3
					return "~";
				case Keys.OemOpenBrackets: //oem4
					return "[";
				case Keys.OemPipe: //oem5
					return "|";
				case Keys.OemCloseBrackets: //oem6
					return "]";
				case Keys.OemQuotes: //oem7
					return "'";
				case Keys.OemBackslash: //oem102
					return "/";
				case Keys.Oemplus:
					return "+";
				case Keys.OemMinus:
					return "-";
				case Keys.Oemcomma:
					return ",";
				case Keys.OemPeriod:
					return ".";

				//unsupported oem keys
				case Keys.Oem8:
				case Keys.OemClear:
					return null;

				//unsupported other keys
				case Keys.None:
				case Keys.LButton:
				case Keys.RButton:
				case Keys.MButton:
				case Keys.XButton1:
				case Keys.XButton2:
				case Keys.Clear:
				case Keys.Sleep:
				case Keys.Cancel:
				case Keys.LineFeed:
				case Keys.Select:
				case Keys.Print:
				case Keys.Execute:
				case Keys.Separator:
				case Keys.ProcessKey:
				case Keys.Packet:
				case Keys.Attn:
				case Keys.Crsel:
				case Keys.Exsel:
				case Keys.EraseEof:
				case Keys.NoName:
				case Keys.Pa1:
				case Keys.KeyCode:
				case Keys.Modifiers:
					return null;

				default:
					throw new NotSupportedException(Enum.GetName(typeof(Keys), key));
			}
		}
	}
}