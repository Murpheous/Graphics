<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class FrmIf
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents StartRectOld As System.Windows.Forms.Button
	Public WithEvents StartRect As System.Windows.Forms.Button
	Public WithEvents btnSave As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.StartRectOld = New System.Windows.Forms.Button()
        Me.StartRect = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.PicResult = New System.Windows.Forms.PictureBox()
        Me.TxtName = New System.Windows.Forms.TextBox()
        Me.lblComplete = New System.Windows.Forms.Label()
        CType(Me.PicResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StartRectOld
        '
        Me.StartRectOld.BackColor = System.Drawing.SystemColors.Control
        Me.StartRectOld.Cursor = System.Windows.Forms.Cursors.Default
        Me.StartRectOld.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartRectOld.ForeColor = System.Drawing.SystemColors.ControlText
        Me.StartRectOld.Location = New System.Drawing.Point(232, 0)
        Me.StartRectOld.Name = "StartRectOld"
        Me.StartRectOld.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartRectOld.Size = New System.Drawing.Size(89, 33)
        Me.StartRectOld.TabIndex = 5
        Me.StartRectOld.Text = "Start Rectangular"
        Me.StartRectOld.UseVisualStyleBackColor = False
        '
        'StartRect
        '
        Me.StartRect.BackColor = System.Drawing.SystemColors.Control
        Me.StartRect.Cursor = System.Windows.Forms.Cursors.Default
        Me.StartRect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartRect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.StartRect.Location = New System.Drawing.Point(136, 0)
        Me.StartRect.Name = "StartRect"
        Me.StartRect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartRect.Size = New System.Drawing.Size(89, 33)
        Me.StartRect.TabIndex = 4
        Me.StartRect.Text = "Start Rectangular"
        Me.StartRect.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.SystemColors.Control
        Me.btnSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSave.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSave.Location = New System.Drawing.Point(24, 56)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSave.Size = New System.Drawing.Size(89, 25)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'PicResult
        '
        Me.PicResult.BackColor = System.Drawing.Color.Black
        Me.PicResult.Location = New System.Drawing.Point(0, 0)
        Me.PicResult.Name = "PicResult"
        Me.PicResult.Size = New System.Drawing.Size(978, 634)
        Me.PicResult.TabIndex = 6
        Me.PicResult.TabStop = False
        Me.PicResult.UseWaitCursor = True
        '
        'TxtName
        '
        Me.TxtName.Location = New System.Drawing.Point(170, 61)
        Me.TxtName.Name = "TxtName"
        Me.TxtName.Size = New System.Drawing.Size(100, 20)
        Me.TxtName.TabIndex = 7
        '
        'lblComplete
        '
        Me.lblComplete.AutoSize = True
        Me.lblComplete.Font = New System.Drawing.Font("Arial Rounded MT Bold", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComplete.Location = New System.Drawing.Point(30, 19)
        Me.lblComplete.Name = "lblComplete"
        Me.lblComplete.Size = New System.Drawing.Size(82, 22)
        Me.lblComplete.TabIndex = 8
        Me.lblComplete.Text = "Percent"
        '
        'FrmIf
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(696, 525)
        Me.Controls.Add(Me.lblComplete)
        Me.Controls.Add(Me.TxtName)
        Me.Controls.Add(Me.StartRectOld)
        Me.Controls.Add(Me.StartRect)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.PicResult)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "FrmIf"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = " Interference"
        CType(Me.PicResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PicResult As System.Windows.Forms.PictureBox
    Friend WithEvents TxtName As System.Windows.Forms.TextBox
    Friend WithEvents lblComplete As System.Windows.Forms.Label
#End Region 
End Class