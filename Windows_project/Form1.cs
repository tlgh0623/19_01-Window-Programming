using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Process[] processes = Process.GetProcesses();      // 프로세스 객체 생성
        string selectpro;    // 선택한프로세스
        string[] info = null;  
        ListViewItem listviewItems = new ListViewItem();    // 리스트뷰 객체 생성
        infoClass information = new infoClass();
        

        public Form1()
        {
            InitializeComponent();
            imageList1.ImageSize = new Size(24, 24);    // 아이콘 사이즈
            listView1.SmallImageList = imageList1;
            label1.Text = "실행중인 프로세스 수 : " + Convert.ToString(processes.Length);
            info = information.process_Load();
        }

        private void info_Load()
        {

            listView1.Items.Clear();
            foreach (Process process in processes)
            {
                try
                {
                    Icon ico = Icon.ExtractAssociatedIcon(process.MainModule.FileName);     // 실행중인 프로세스 아이콘 추출
                    imageList1.Images.Add(Icon.ExtractAssociatedIcon(process.MainModule.FileName)); // 이미지리스트에 아이콘 추가
                    listView1.Items.Add(process.ProcessName, imageList1.Images.Count -1);    // 리스트뷰에 아이템 추가
                }
                catch
                {
                    listView1.Items.Add(process.ProcessName, imageList1.Images.Count - 1);
                }
            }
        }
        private void btnseach_Click(object sender, EventArgs e)     // 새로 고침
        {
            info_Load();
        }

        private void Form1_Load(object sender, EventArgs e)     
        {
            info_Load();
            
        }

        private void btnprocessclose_Click(object sender, EventArgs e)      // 프로세스 종료
        {
            try
            {
                string pro = listView1.SelectedItems[0].SubItems[0].Text;   // 리스트뷰에서 선택한 프로세스
                foreach (Process process in Process.GetProcesses()) // 프로세스 목록
                {
                    if (process.ProcessName.StartsWith(pro))    // 해당 프로세스가 있으면 종료 후 새로고침
                    {
                        if (MessageBox.Show("선택한 " + pro + " 를 종료할까요?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            MessageBox.Show(pro + " 를 종료합니다.");
                            process.Kill();
                            btnseach_Click(sender, e);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("선택한 프로세스가 없습니다");
            }
        }

        private void btnclose_Click(object sender, EventArgs e)     // 프로그램 종료
        {
            Application.Exit();
        }

        private void Button1_Click(object sender, EventArgs e)  // 선택한 프로세스가 실행중이지 않다면 컴퓨터 종료
        {
            // 선택한 프로세스 이름 저장
            timer1.Interval = 5000;     // 타이머 간격
            timer1.Start(); // 타이머 시작

            try
            {
                this.selectpro = listView1.SelectedItems[0].SubItems[0].Text;
                foreach (Process process in Process.GetProcesses()) // 프로세스 목록
                {
                    if (process.ProcessName.StartsWith(selectpro))    // 해당 프로세스가 있는지
                    {
                        MessageBox.Show("선택한 프로세스가 꺼지면 컴퓨터가 종료됩니다.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("선택한 프로세스가 없습니다");
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)    // 타이머
        {
            Process[] processList = Process.GetProcessesByName(selectpro);      // 프로세스의 이름에 대한 정보를 프로세스 변수에 넣음

            if (processList.Length < 1)     // 실행중이지 않다면 명령어 실행후 타이머 스탑
            {
                Process.Start("shutdown.exe", "-s -t 60");  // 컴퓨터 종료 명령어 실행(1분 설정)
                timer1.Stop();
                //MessageBox.Show("프로세스가 종료되었습니다 \r\n컴퓨터를 종료합니다.");
            }
        }
    }
}
