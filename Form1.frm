VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   4920
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   11565
   LinkTopic       =   "Form1"
   ScaleHeight     =   4920
   ScaleWidth      =   11565
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command2 
      Caption         =   "Call Form2"
      Height          =   615
      Left            =   4680
      TabIndex        =   2
      Top             =   3840
      Width           =   4215
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Exit"
      Height          =   495
      Left            =   1200
      TabIndex        =   1
      Top             =   3840
      Width           =   3015
   End
   Begin VB.OLE OLE1 
      Class           =   "Excel.OpenDocumentSpreadsheet.12"
      Height          =   2415
      Left            =   1290
      OleObjectBlob   =   "Form1.frx":0000
      TabIndex        =   0
      Top             =   930
      Width           =   7455
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
    Call Unload(Me)
    End
End Sub

Private Sub Command2_Click()
    Dim f As New Form2
    f.Show
    Call Unload(Me)
End Sub

Private Sub Form_Load()
  Form1.Caption = "hi"
End Sub

