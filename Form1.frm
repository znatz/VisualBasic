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
Private Sub Form_Load()
  Form1.Caption = "hi"
End Sub

