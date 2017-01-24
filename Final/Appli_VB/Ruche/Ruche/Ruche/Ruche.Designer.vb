<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Ruche
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Ruche))
        Me.btn_Quitter = New System.Windows.Forms.PictureBox()
        Me.lbl_Creator = New System.Windows.Forms.Label()
        Me.IconeAbeille = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.btn_Effacer = New System.Windows.Forms.Button()
        Me.btn_BaseDeDonnees = New System.Windows.Forms.Button()
        Me.rtb_Donnees = New System.Windows.Forms.RichTextBox()
        Me.lbl_Titre = New System.Windows.Forms.Label()
        Me.PortSerieIN = New System.IO.Ports.SerialPort(Me.components)
        CType(Me.btn_Quitter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_Quitter
        '
        Me.btn_Quitter.ImageLocation = ""
        Me.btn_Quitter.Location = New System.Drawing.Point(99, 217)
        Me.btn_Quitter.Name = "btn_Quitter"
        Me.btn_Quitter.Size = New System.Drawing.Size(107, 28)
        Me.btn_Quitter.TabIndex = 1
        Me.btn_Quitter.TabStop = False
        Me.btn_Quitter.Tag = "Btn_quitter"
        '
        'lbl_Creator
        '
        Me.lbl_Creator.AutoSize = True
        Me.lbl_Creator.Location = New System.Drawing.Point(9, 268)
        Me.lbl_Creator.Name = "lbl_Creator"
        Me.lbl_Creator.Size = New System.Drawing.Size(39, 13)
        Me.lbl_Creator.TabIndex = 2
        Me.lbl_Creator.Text = "Label1"
        '
        'IconeAbeille
        '
        Me.IconeAbeille.Icon = CType(resources.GetObject("IconeAbeille.Icon"), System.Drawing.Icon)
        Me.IconeAbeille.Text = "NotifyIcon1"
        Me.IconeAbeille.Visible = True
        '
        'btn_Effacer
        '
        Me.btn_Effacer.Location = New System.Drawing.Point(12, 217)
        Me.btn_Effacer.Name = "btn_Effacer"
        Me.btn_Effacer.Size = New System.Drawing.Size(75, 23)
        Me.btn_Effacer.TabIndex = 3
        Me.btn_Effacer.Text = "Button1"
        Me.btn_Effacer.UseVisualStyleBackColor = True
        '
        'btn_BaseDeDonnees
        '
        Me.btn_BaseDeDonnees.Location = New System.Drawing.Point(213, 217)
        Me.btn_BaseDeDonnees.Name = "btn_BaseDeDonnees"
        Me.btn_BaseDeDonnees.Size = New System.Drawing.Size(75, 23)
        Me.btn_BaseDeDonnees.TabIndex = 4
        Me.btn_BaseDeDonnees.Text = "Button2"
        Me.btn_BaseDeDonnees.UseVisualStyleBackColor = True
        '
        'rtb_Donnees
        '
        Me.rtb_Donnees.Location = New System.Drawing.Point(106, 62)
        Me.rtb_Donnees.Name = "rtb_Donnees"
        Me.rtb_Donnees.Size = New System.Drawing.Size(100, 104)
        Me.rtb_Donnees.TabIndex = 5
        Me.rtb_Donnees.Text = ""
        '
        'lbl_Titre
        '
        Me.lbl_Titre.AutoSize = True
        Me.lbl_Titre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Titre.Location = New System.Drawing.Point(84, 16)
        Me.lbl_Titre.Name = "lbl_Titre"
        Me.lbl_Titre.Size = New System.Drawing.Size(39, 13)
        Me.lbl_Titre.TabIndex = 6
        Me.lbl_Titre.Text = "Label1"
        '
        'PortSerieIN
        '
        '
        'Ruche
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(308, 290)
        Me.Controls.Add(Me.lbl_Titre)
        Me.Controls.Add(Me.rtb_Donnees)
        Me.Controls.Add(Me.btn_BaseDeDonnees)
        Me.Controls.Add(Me.btn_Effacer)
        Me.Controls.Add(Me.lbl_Creator)
        Me.Controls.Add(Me.btn_Quitter)
        Me.Name = "Ruche"
        Me.Text = "Form1"
        CType(Me.btn_Quitter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btn_Quitter As System.Windows.Forms.PictureBox
    Friend WithEvents lbl_Creator As System.Windows.Forms.Label
    Friend WithEvents IconeAbeille As System.Windows.Forms.NotifyIcon
    Friend WithEvents btn_Effacer As System.Windows.Forms.Button
    Friend WithEvents btn_BaseDeDonnees As System.Windows.Forms.Button
    Friend WithEvents rtb_Donnees As System.Windows.Forms.RichTextBox
    Friend WithEvents lbl_Titre As System.Windows.Forms.Label
    Friend WithEvents PortSerieIN As System.IO.Ports.SerialPort

End Class
