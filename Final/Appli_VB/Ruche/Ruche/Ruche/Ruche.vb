Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing.Imaging
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Imports Microsoft.Office.Interop

Imports System.Data.OleDb
Imports System.Data.Sql

Imports System.Threading
Imports System.IO.Ports
Imports System.ComponentModel

Public Class Ruche
    Dim CenterX As Integer
    Dim CenterY As Integer
    Dim MaxFont As Integer = 1
    Dim TailleLabel As Integer
    Dim myPort As Array 'myPort est un tableau, il va servir pour la communication.
    Dim Compteur As Integer

    Private Sub Ruche_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        On Error GoTo Err_Communication 'Si erreur, aller à "Err_Handler"

        'Config du Form
        IconeAbeille.Icon = New System.Drawing.Icon("E:\Ruche\Ruche\Images\Abeille.ico") 'Icone de l'appli
        IconeAbeille.Visible = True
        IconeAbeille.Text = "Ruche"

        Me.Icon = IconeAbeille.Icon
        Me.Text = "Ruche" 'Titre de l'appli
        Me.BackgroundImage = System.Drawing.Image.FromFile("E:\Ruche\Ruche\Images\FondEcran.png") 'Image de fond
        Me.Size = New Size(730, 490) 'Taille de la page (du Form)
        Me.WindowState = FormWindowState.Maximized 'Application en plein écran
        Me.BackgroundImageLayout = ImageLayout.Stretch 'Etirement du l'image par rapport au Form
        '-------------------------------------------------------------------------------------

        'Servira pour plus tard
        MaxFont = Me.Size.Width 'On enregistre la taille maximum du Form (Plein écran)
        '-------------------------------------------------------------------------------------

        'Config des boutons et des labels
        btn_Quitter.Text = "Quitter"
        btn_Quitter.BackgroundImage = System.Drawing.Image.FromFile("E:\Ruche\Ruche\Images\hyhnd.png") 'Lien vers l'image à afficher
        btn_Quitter.Size = New Size(138, 43) 'Taille du bouton
        CenterX = (Me.Size.Width / 2) - (btn_Quitter.Size.Width / 2) 'X du bouton par rapport au X du Form
        CenterY = (Me.Size.Height - 150) - (btn_Quitter.Size.Height / 2) 'Y du bouton par rapport au Y du Form
        btn_Quitter.Location = New Point(CenterX, CenterY) 'Mouvement du bouton avec le Form
        btn_Quitter.BackgroundImageLayout = ImageLayout.Stretch 'Etirer l'image à la taille demandée [Btn_Quitter.Size = New Size(138, 43)] 
        btn_Quitter.BackColor = Color.Transparent 'Eviter un carré blanc derrière l'écran
        btn_Quitter.Focus()

        btn_Effacer.Text = "Effacer"
        btn_Effacer.Size = New Size(138, 43) 'Taille du bouton
        CenterX = (Me.Size.Width / 4) - (btn_Quitter.Size.Width / 2) 'X du bouton par rapport au X du Form
        CenterY = (Me.Size.Height - 150) - (btn_Quitter.Size.Height / 2) 'Y du bouton par rapport au Y du Form
        btn_Effacer.Location = New Point(CenterX, CenterY) 'Mouvement du bouton avec le Form
        btn_Effacer.ForeColor = Color.Orange 'Couleur bouton
        btn_Effacer.BackColor = Color.BlueViolet
        btn_Effacer.FlatStyle = FlatStyle.Popup 'Type de bouton
        btn_Effacer.Font = New Font("Comic Sans MS", 11, FontStyle.Bold) 'Ecriture du bouton

        btn_BaseDeDonnees.Text = "Base de données"
        btn_BaseDeDonnees.Size = New Size(138, 43) 'Taille du bouton
        CenterX = (Me.Size.Width / 4 * 3) - (btn_Quitter.Size.Width / 2) 'X du bouton par rapport au X du Form
        CenterY = (Me.Size.Height - 150) - (btn_Quitter.Size.Height / 2) 'Y du bouton par rapport au Y du Form
        btn_BaseDeDonnees.Location = New Point(CenterX, CenterY) 'Mouvement du bouton avec le Form
        btn_BaseDeDonnees.ForeColor = Color.Orange
        btn_BaseDeDonnees.BackColor = Color.BlueViolet
        btn_BaseDeDonnees.FlatStyle = FlatStyle.Popup
        btn_BaseDeDonnees.Font = New Font("Comic Sans MS", 11, FontStyle.Bold)

        'Réglage de la zone de texte
        rtb_Donnees.Enabled = False 'Empeche l'écriture
        rtb_Donnees.Size = New Size((Me.Size.Width - 200), (Me.Size.Height - 300)) 'Taille
        rtb_Donnees.Location = New Point(100, 100) 'Emplacement

        lbl_Creator.Text = "By Nora"
        lbl_Creator.Font = New Font(lbl_Creator.Text, 13.0, FontStyle.Italic)
        CenterX = 50 'X du bouton par rapport au X du Form
        CenterY = Me.Size.Height - 100 'Y du bouton par rapport au Y du Form
        lbl_Creator.Location = New Point(CenterX, CenterY) 'Mouvement du label avec le Form
        lbl_Creator.BackColor = Color.Transparent 'Eviter d'avoir un rectangle blanc derrière l'écriture

        lbl_Titre.Text = "Nombre d'abeilles à l'intérieur de la ruche :"
        TailleLabel = Me.Size.Width / MaxFont * 35 'Réglage de la taille du label en fonction de la taille maxi du Form
        lbl_Titre.Font = New Font("Segoe Script", TailleLabel, FontStyle.Bold And FontStyle.Underline)
        lbl_Titre.ForeColor = Color.BlueViolet
        CenterX = (Me.Size.Width / 2) - (lbl_Titre.Size.Width / 2)
        CenterY = 25 'Y du bouton par rapport au Y du Form
        lbl_Titre.Location = New Point(CenterX, CenterY) 'Mouvement du label avec le Form
        lbl_Titre.BackgroundImageLayout = ImageLayout.Stretch
        lbl_Titre.BackColor = Color.Transparent
        '-------------------------------------------------------------------------------------

        'Communication
        myPort = IO.Ports.SerialPort.GetPortNames() 'On entre toute les ports séries connectés dans le tableau "myPort"
        Compteur = 0
        Dim TestCOM As String
        TestCOM = myPort(Compteur)


        'On affiche sur quel port sur laquel nous sommes connectés
        rtb_Donnees.Text = vbCrLf & "Vous êtes maintenant connecté au port : " & myPort(Compteur) & rtb_Donnees.Text & vbCrLf

        PortSerieIN.Close()
        PortSerieIN.PortName = myPort(Compteur) 'Sélection du port
        PortSerieIN.BaudRate = 9600 'Sélection du baud rate
        PortSerieIN.Parity = IO.Ports.Parity.None 'Sélection de la parité
        PortSerieIN.StopBits = IO.Ports.StopBits.One 'Sélection des bits de stop
        PortSerieIN.DataBits = 8 'Sélection des bits de données
        PortSerieIN.Encoding = System.Text.Encoding.Default
        PortSerieIN.Open() 'On ouvre le port, la communication peut commencer

        Exit Sub

Err_Communication:
        Dim Choix As Integer

        Choix = MsgBox("Veuillez rebrancher la puce XBee au PC, attendre 5 secondes puis retester" & vbCrLf & "Appuyez sur 'OK' pour retester", 1, "Problème de communication")
        'exemple vaut maintenant le chiffre correspondant au bouton appuyé

        If Choix = 1 Then 'Si "OK"
            myPort = IO.Ports.SerialPort.GetPortNames() 'On entre toute les ports séries connectés dans le tableau "myPort"
            Resume 'Sert à revenir au moment où l'erreur est survenue
        ElseIf Choix = 2 Then 'Si "Annuler"
            End
        End If

    End Sub
    Private Sub PortSerieIN_DataReceived1(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles PortSerieIN.DataReceived
        Threading.Thread.Sleep(500) 'Fonction permettant une attente de 500ms
        ReceivedText(PortSerieIN.ReadExisting()) 'Reception des données

    End Sub

    Delegate Sub SetTextCallback(ByVal NbrAbeilles As String)

    Private Sub ReceivedText(ByVal NbrAbeilles As String) 'input from ReadExisting
        'Cette fonction sert à afficher les donneées recus dans la zone de texte de réception
        If Me.rtb_Donnees.InvokeRequired Then
            Dim x As New SetTextCallback(AddressOf ReceivedText)
            Me.Invoke(x, New Object() {(NbrAbeilles)})
        Else
            rtb_Donnees.Text = vbCrLf & NbrAbeilles & rtb_Donnees.Text & vbCrLf 'Texte à noter sur la zone de données
            ReceptionExcel(NbrAbeilles)
        End If
    End Sub

    Private Sub ReceptionExcel(ByVal NbrAbeillesActuel As String)


        On Error GoTo Err_FichierExcel1 'Si erreur, aller à "Err_Handler"

        
        'Assemblage des données dans le fichier excel
        Dim appXL As Excel.Application
        Dim wbXl As Excel.Workbook
        Dim shXL As Excel.Worksheet

        ' On démarre l'application excel sans la rendre visible
        appXL = CreateObject("Excel.Application")
        wbXl = appXL.Workbooks.Open("E:\Base_de_donnees.xlsx")
        shXL = wbXl.ActiveSheet

        ' Si le fichier excel est encore vierge, mise des titres de colonnes
        If shXL.Cells(1, 1).Value = "" Then ' si la case du titre est vide alors
            shXL.Columns("A").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter 'Le texte se trouve au milieu de la case
            shXL.Columns("A").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            shXL.Cells(1, 1).Value = "Date" 'On note "Date"
            shXL.Columns("A").Font.Bold = False 'On ne met que la premiere case en gras, le reste en normal
            shXL.Cells(1, 1).Font.Bold = True
        End If
        If shXL.Cells(1, 2).Value = "" Then
            shXL.Columns("B").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            shXL.Columns("B").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            shXL.Cells(1, 2).Value = "Heure"
            shXL.Columns("B").Font.Bold = False
            shXL.Cells(1, 2).Font.Bold = True
        End If
        If shXL.Cells(1, 3).Value = "" Then
            shXL.Columns("C").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            shXL.Columns("C").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            shXL.Cells(1, 3).Value = "Nombre d'Abeilles"
            shXL.Columns("C").Font.Bold = False
            shXL.Cells(1, 3).Font.Bold = True
        End If
        shXL.Rows(2).Insert()
        shXL.Cells(2, 1).Value = Date.Today
        shXL.Cells(2, 2).Value = TimeOfDay
        shXL.Cells(2, 3).Value = NbrAbeillesActuel

        'On ferme tout
        wbXl.Save()
        wbXl.Close()
        appXL.Workbooks.Close()
        appXL.Quit()
        GC.Collect() 'Empeche l'ouverture du plus d'un fichier excel

        PortSerieIN.Open() 'On ouvre le port, la communication peut commencer

        Exit Sub

Err_FichierExcel1:
        Dim Choix As Integer ' "OK" ou "Annuler" selon le choix de l'utilisateur

        Choix = MsgBox("Veuillez revérifier ces points-ci :" & vbCrLf &
                       "    - Que la clé USB se nomme 'USB_RUCHE' sous le disque 'F:'" & vbCrLf &
                       "    - Qu'il y ait un fichier excel dans la clé USB" & vbCrLf &
                       "    - Que ce fichier excel se nomme 'Base_de_donnees' sous format '.xlsx'" & vbCrLf &
                       vbCrLf & "Appuyez sur 'OK' pour retester", 1, "Fichier excel introuvable")
        'Choix = MsgBox(Message à dire, Type de MsgBox, Titre de la MsgBox)

        If Choix = 1 Then 'Si "OK"

            Resume 'Sert à revenir au moment où l'erreur est survenue
        ElseIf Choix = 2 Then 'Si "Annuler"
            End 'Quitter l'application
        End If


    End Sub

    'Si changement de taille du form :
    Private Sub Ruche_ClientSizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ClientSizeChanged

        Me.MinimumSize = New System.Drawing.Size(730, 490) 'On ne peut pas rétrécir plus 

        CenterX = (Me.Size.Width / 2) - (btn_Quitter.Size.Width / 2) 'X du bouton par rapport au X du Form
        CenterY = (Me.Size.Height - 150) - (btn_Quitter.Size.Height / 2) 'Y du bouton par rapport au Y du Form
        btn_Quitter.Location = New Point(CenterX, CenterY) 'Mouvement du bouton avec le Form

        CenterX = (50) 'X du label par rapport au X du Form
        CenterY = Me.Size.Height - 100 'Y du label par rapport au Y du Form
        lbl_Creator.Location = New Point(CenterX, CenterY) 'Mouvement du label avec le Form

        CenterX = (Me.Size.Width / 4) - (btn_Quitter.Size.Width / 2) 'X du bouton par rapport au X du Form
        CenterY = (Me.Size.Height - 150) - (btn_Quitter.Size.Height / 2) 'Y du bouton par rapport au Y du Form
        btn_Effacer.Location = New Point(CenterX, CenterY) 'Mouvement du bouton avec le Form

        CenterX = (Me.Size.Width / 4 * 3) - (btn_Quitter.Size.Width / 2) 'X du bouton par rapport au X du Form
        CenterY = (Me.Size.Height - 150) - (btn_Quitter.Size.Height / 2) 'Y du bouton par rapport au Y du Form
        btn_BaseDeDonnees.Location = New Point(CenterX, CenterY) 'Mouvement du bouton avec le Form

        rtb_Donnees.Size = New Size((Me.Size.Width - 200), (Me.Size.Height - 300)) 'Réglage zone de texte


        TailleLabel = Me.Size.Width / MaxFont * 35 'Réglage de la taille du titre en fonction de la taille de la page
        lbl_Titre.Font = New Font("Segoe Script", TailleLabel, FontStyle.Underline And FontStyle.Bold) 'Config police
        CenterX = (Me.Size.Width / 2) - (lbl_Titre.Size.Width / 2)
        CenterY = 25
        lbl_Titre.Location = New Point(CenterX, CenterY) 'Mouvement du label en fonction du Form

    End Sub

    Private Sub Ruche_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btn_Quitter_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Quitter.MouseEnter
        Me.Cursor = Cursors.Hand
    End Sub

    Private Sub btn_Quitter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Quitter.Click
        IconeAbeille.Visible = False
        End 'Fin du programme
    End Sub

    Private Sub btn_Effacer_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Effacer.MouseEnter
        Me.Cursor = Cursors.Hand 'La souris devient une main si on se met sur le bouton
    End Sub

    Private Sub btn_Effacer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Effacer.Click
        rtb_Donnees.Text = ""
    End Sub

    Private Sub btn_BaseDeDonnees_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BaseDeDonnees.MouseEnter
        Me.Cursor = Cursors.Hand
    End Sub

    Private Sub btn_BaseDeDonnees_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BaseDeDonnees.Click

        PortSerieIN.Close() 'Coupe la communication pour éviter que le fichier excel soit ouvert par l'utilisateur en même temps que par l'application

        On Error GoTo Err_FichierExcel2 'Si erreur, aller à "Err_Handler"

        '  Add the following code snippet on top of Form1.vb

        Dim appXL As Excel.Application 'On ouvre l appli
        Dim wbXl As Excel.Workbook
        appXL = CreateObject("Excel.Application")
        appXL.Visible = True 'On la rend visible
        wbXl = appXL.Workbooks.Open("E:\Base_de_donnees.xlsx") 'On ouvre ce fichier dans l'appli

        PortSerieIN.Open() 'On relance la communication

        Exit Sub


Err_FichierExcel2:

        appXL.WindowState = Excel.XlWindowState.xlMinimized 'On réduit la page excel pour ne pas cacher le MsgBox qui vient ensuite

        Dim Choix As Integer ' "OK" ou "Annuler" selon le choix de l'utilisateur

        Choix = MsgBox("Veuillez revérifier ces points-ci :" & vbCrLf &
                       "    - Que la clé USB se nomme 'USB_RUCHE' sous le disque 'F:'" & vbCrLf &
                       "    - Qu'il y ait un fichier excel dans la clé USB" & vbCrLf &
                       "    - Que ce fichier excel se nomme 'Base_de_donnees' sous format '.xlsx'" & vbCrLf &
                       vbCrLf & "Appuyez sur 'OK' pour retester", 1, "Fichier excel introuvable")
        'Choix = MsgBox(Message à dire, Type de MsgBox, Titre de la MsgBox)

        If Choix = 1 Then 'Si "OK"

            Resume 'Sert à revenir au moment où l'erreur est survenue
        ElseIf Choix = 2 Then 'Si "Annuler"
            End 'Quitter l'application
        End If

    End Sub


End Class