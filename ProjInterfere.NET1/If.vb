Option Strict Off
Option Explicit On
Friend Class FrmIf

    Inherits System.Windows.Forms.Form
    Public MyBitmap As Bitmap
    Public MyGraphics As Graphics
    Dim RunningMax As Double
    Public Const nDeltas As Integer = nStartScreenPoints / StartSpan
    Public Const nPixelsHigh As Integer = 3600

    Public Const nPixelsWide As Short = 15360


    Public Const StartMagnification As Short = 2
    ' Number of points in start map
    Public Const nStartScreenPoints As Integer = (nPixelsHigh * StartMagnification)

    Public Const SlitSigma As Short = 195
    Public Const xStepPerPixel As Integer = 1.25
    ' Two slit setting

    Public Const nSlits As Short = 2
    Public Const StartSpan As Double = 0.0319444 ' Amount of screen used by start map
    Public Const yPixelsPerStartStep As Double = StartSpan / StartMagnification

    Public Const Lambda As Double = 0.44

    ' Big pic dimensions



    Public Const Pi As Double = 3.14159265358979
    Public Const TwoPi As Double = 6.28318530717959


    Public PathLens(nDeltas) As Double
    Public Sines(nDeltas) As Double
    Public Cosines(nDeltas) As Double


    Public Const TwoPi_Lambda As Double = TwoPi / Lambda

    Structure DensRec
        Dim Prob As Double
        'CumProb As Double
    End Structure

    Structure clPhase
        Dim X As Double
        Dim Y As Double
    End Structure

    Public StartMap(nStartScreenPoints + 4) As DensRec
    Public PhaseSum(nPixelsHigh + 2) As clPhase


    Public FirstMaxPixels As Double
    Public Map(nPixelsHigh + 1, nPixelsWide + 1) As Double

    Public StopIt As Boolean
    Public MapMaxVal(nPixelsWide + 1, 1) As Double

    Public Function GaussianVal(ByVal ArgMean As Integer, ByVal ArgSigma As Integer, ByVal ArgX As Integer) As Double
        Dim TwoSigSq As Double
        TwoSigSq = 2 * ArgSigma * ArgSigma
        'Dim G As Double
        'G = 1 / Sqr(Pi * TwoSigSq)
        Dim X As Double
        X = ArgX - ArgMean
        X = X * X

        'EXP(-POWER(($A2-I$2),2)/$G$3)
        GaussianVal = System.Math.Exp(-X / TwoSigSq)
    End Function
    Public Sub EraseMap()
        Dim i, j As Integer
        For i = 0 To nPixelsWide
            MapMaxVal(i, 0) = 0
            MapMaxVal(i, 1) = 0
            For j = 0 To nPixelsHigh
                Map(j, i) = 0
            Next j
        Next i
        Dim k As Integer
        For k = 0 To nStartScreenPoints
            ZeroDensRec(StartMap(k))
        Next k
    End Sub
    Public Sub InitMaps(ByVal IsRectangle As Boolean)
        Dim nCount As Integer
        Dim nMySlit As Integer
        Dim SlitOffset As Integer
        Dim nMean As Integer
        Dim nSigma As Integer
        EraseMap()
        Dim nSlitPoints As Integer
        Dim TotalSlitPonts As Integer
        Dim SlitGap As Integer
        nSigma = StartMagnification * SlitSigma
        If (IsRectangle) Then
            nSlitPoints = nSigma * 2
        Else
            nSlitPoints = nSigma * 8 ' Four sigma width
        End If
        nMean = nSlitPoints / 2
        'nSlitPoints = nStartScreenPoints * SlitRatio

        TotalSlitPonts = nSlitPoints * nSlits
        If (nSlits > 1) Then
            SlitGap = (nStartScreenPoints - TotalSlitPonts) / (nSlits - 1)
        Else
            SlitGap = 0
        End If

        SlitOffset = 0
        ' Zero the map
        For nCount = 1 To nStartScreenPoints
            StartMap(nCount).Prob = 0.0#
        Next nCount
        If (nSlits = 1) Then
            SlitOffset = (nStartScreenPoints / 2) - (nSlitPoints / 2)
        End If
        If (IsRectangle) Then
            For nMySlit = 1 To nSlits
                For nCount = 0 To nSlitPoints
                    StartMap(nCount + SlitOffset).Prob = 1
                Next nCount
                SlitOffset = (SlitGap + nSlitPoints) * nMySlit
            Next nMySlit
        Else
            For nMySlit = 1 To nSlits
                For nCount = 0 To nSlitPoints
                    StartMap(nCount + SlitOffset).Prob = StartMap(nCount + SlitOffset).Prob + GaussianVal(nMean, nSigma, nCount)
                Next nCount
                SlitOffset = (SlitGap + nSlitPoints) * nMySlit
            Next nMySlit
        End If
    End Sub
    'Public Sub SingleSlitEndMap(MaxTheta As Double)
    '
    'Dim Count As Integer
    'Dim MyAngle As Double, MyScale As Double, Result As Double, L1 As Double, L2 As Double, Tmp As Double, hPix As Double
    'Dim Idx1 As Long, Idx2 As Long

    '  MyScale = (MaxTheta / nPixelsHigh) * 2#
    '  Count = nPixelsHigh / 2#
    '  Idx1 = 0
    '  Idx2 = nPixelsHigh
    '  FirstMaxPixels = Pi / (4.5 * MyScale)
    '  'hPix = hPixels
    '  Tmp = FirstMaxPixels - (SlitSeparation / 2#)
    '  L1 = Sqr(Tmp * Tmp + hPix * hPix)
    '  Tmp = FirstMaxPixels + (SlitSeparation / 2#)
    '  L2 = Sqr(Tmp * Tmp + hPix * hPix)
    '  Lambda = L2 - L1
    '  '//Result = FirstMaxPixels/hPixels;       // Slope to first max
    '  '//Result = atan2(Result,1.0);            // Theta to first max
    '  '//Lambda = SlitSeparation * sin(Result); // Lambda = dSin(theta);
    '  While (Count >= 0)
    '    If Count = 0 Then
    '       Result = 1
    '    Else
    '      MyAngle = MyScale * Count
    '      Result = Sin(MyAngle) / MyAngle
    '    End If
    '    Result = Result * Cos(MyAngle * 4.5)'
    '
    '    Result = Result * Result
    '    Count = Count - 1
    ' '   Idx1 = Idx1 + 1
    '    Idx2 = Idx2 - 1
    '  Wend
    'End Sub


    Public Sub ZeroDensRec(ByRef argRec As DensRec)
        'argRec.CumProb = 0
        argRec.Prob = 0
    End Sub

    Public Sub ZeroPhase(ByRef argPhase As clPhase)
        argPhase.X = 0.0#
        argPhase.Y = 0.0#
    End Sub

    Public Function AmplitudeSquared(ByRef argPhase As clPhase) As Double
        AmplitudeSquared = argPhase.X * argPhase.X + argPhase.Y * argPhase.Y
    End Function

    Public Function Amplitude(ByRef argPhase As clPhase) As Double
        Amplitude = System.Math.Sqrt(argPhase.X * argPhase.X + argPhase.Y * argPhase.Y)
    End Function

    Public Sub AddPhase(ByRef argPhase As clPhase, ByRef Theta As Double, ByRef Amp As Double)

        argPhase.X = argPhase.X + (Amp * System.Math.Cos(Theta))
        argPhase.Y = argPhase.Y + (Amp * System.Math.Sin(Theta))

    End Sub

    Public Sub AddPhaseNew(ByRef argPhase As clPhase, ByRef AmpSinTheta As Double, ByRef AmpCosTheta As Double)
        argPhase.X = argPhase.X + AmpCosTheta
        argPhase.Y = argPhase.Y + AmpSinTheta
    End Sub


    Private Sub btnSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnSave.Click
        'UPGRADE_ISSUE: PicResult was upgraded to a Panel, and cannot be coerced to a PictureBox. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0FF1188E-27C0-4FED-842D-159C65894C9B"'
        ShowMap(PicResult, nPixelsWide, 0, 0)
        Dim MyName As String
        MyName = TxtName.Text
        MyBitmap.Save("c:\Pix\" & MyName & "BGR.BMP")
        ShowMap(PicResult, nPixelsWide, 0, 1)
        MyBitmap.Save("c:\Pix\" & MyName & "BRG.bmp")
        'ShowMap(PicResult, nPixelsWide, 0, 2)
        'MyBitmap.Save("c:\Pix\" & MyName & "GRB.bmp")
        ShowMap(PicResult, nPixelsWide, 0, 3)
        MyBitmap.Save("c:\Pix\" & MyName & "GBR.bmp")
        ShowMap(PicResult, nPixelsWide, 0, 4)
        MyBitmap.Save("c:\Pix\" & MyName & "RGB.bmp")
        'howMap(PicResult, nPixelsWide, 0, 5)
        'MyBitmap.Save("c:\Pix\" & MyName & "RBG.bmp")
        'ShowMap(PicResult, nPixelsWide, 1, 6)
        'SavePicture PicResult.Image, "c:\Pix\" & MyName & "Pink.bmp"
        ''ShowMap(PicResult, nPixelsWide, 1, 7)
        'SavePicture PicResult.Image, "c:\Pix\" & MyName & "Other.bmp"
        'ShowMap(PicResult, nPixelsWide, 1, 8)
        'SavePicture(PicResult.Image, "c:\Pix\" & MyName & "Other1.bmp")
        'ShowMap(PicResult, nPixelsWide, 1, 9)
        'SavePicture PicResult.Image, "c:\Pix\" & MyName & "Other2.bmp"

    End Sub

    Private Sub cmdStart_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Dim Iterate, Column, Row, StartIndex, PrevDisplayColumn As Integer
        Row = 0 : Column = 0 : StartIndex = 0 : Iterate = 0 : PrevDisplayColumn = 0


        Dim StartOffs, PathLen As Double
        Dim PhaseSum As clPhase
        ZeroPhase(PhaseSum)

        'EraseMap
        InitMaps(False)

        StopIt = False
        Dim xPerPixelSq As Double
        Dim MaxRow As Integer : MaxRow = nPixelsHigh / 2
        Dim ColDistSqr As Double
        Column = 0
        xPerPixelSq = xStepPerPixel * xStepPerPixel
        While (Column <= nPixelsWide) And (StopIt = False)
            Iterate = Iterate + 1
            Row = 0
            ColDistSqr = Column * Column * xPerPixelSq
            While (Row <= MaxRow) And (StopIt = False)
                ZeroPhase(PhaseSum)
                For StartIndex = 0 To nStartScreenPoints
                    If (StartMap(StartIndex).Prob > 0) Then
                        StartOffs = yPixelsPerStartStep * (StartIndex - (nStartScreenPoints / 2))
                        StartOffs = StartOffs + (nPixelsHigh / 2)
                        PathLen = (StartOffs - Row)
                        PathLen = PathLen * PathLen
                        PathLen = PathLen + ColDistSqr
                        PathLen = System.Math.Sqrt(PathLen)
                        If (PathLen > 0) Then
                            AddPhase(PhaseSum, (TwoPi_Lambda * (PathLen)), StartMap(StartIndex).Prob / PathLen)
                            ' Else
                            '  AddPhase PhaseSum, 0, StartMap(StartIndex).Prob
                        End If
                    Else
                        ' Got a Zero
                    End If

                Next StartIndex
                System.Windows.Forms.Application.DoEvents()
                Map(Row, Column) = AmplitudeSquared(PhaseSum)
                Map(nPixelsHigh - Row, Column) = Map(Row, Column)
                'If (Column < 1500) Then
                If (Map(Row, Column) >= MapMaxVal(Column, 0)) Then
                    MapMaxVal(Column, 0) = Map(Row, Column)
                    'MapMaxVal(Column, 1) = Map(Row, Column)
                End If
                'End If
                Row = Row + 1
            End While
            'If (Column >= 700) Then
            'MapMaxVal(Column, 0) = MapMaxVal(Column - 1, 0)
            'MapMaxVal(Column, 1) = MapMaxVal(Column - 1, 1)
            'End If
            If (Iterate > 1) Or (Column = nPixelsWide) Then
                'UPGRADE_ISSUE: PicResult was upgraded to a Panel, and cannot be coerced to a PictureBox. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0FF1188E-27C0-4FED-842D-159C65894C9B"'
                'ShowMap(PicResult, Column, PrevDisplayColumn)
                PrevDisplayColumn = Column + 1
                Iterate = 0
            End If
            Column = Column + 1
            If Column Mod 100 = 1 Then
                Row = 0
            End If

        End While
    End Sub

    Private Sub FrmIf_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        PicResult.Width = nPixelsWide + 2
        PicResult.Height = (nPixelsHigh) + 4
        lblComplete.Text = "Idle"
        'UPGRADE_ISSUE: Constant vbPixels was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
        'UPGRADE_ISSUE: PictureBox property PicResult.ScaleMode was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        'PicResult..ScaleMode = vbPixels
        'UPGRADE_ISSUE: PictureBox property PicResult.ScaleHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        'PicResult.ScaleHeight = nPixelsHigh / 2
        'UPGRADE_ISSUE: PictureBox property PicResult.ScaleWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        'PicResult.ScaleWidth = nPixelsWide
    End Sub

    Private Sub StartRect_old_Click()
        Dim StartIndex, Row, Column, PrevDisplayColumn As Integer
        Row = 0 : Column = 0 : StartIndex = 0 : PrevDisplayColumn = 0


        Dim PathLen, WavePart As Double
        Dim PhaseSum As clPhase
        Dim nCount As Integer

        ZeroPhase(PhaseSum)

        'EraseMap
        InitMaps(True)
        StopIt = False
        Dim xPerPixelSq As Double
        Dim MaxRow As Integer : MaxRow = (nPixelsHigh / 2) + 1
        Dim ColDistSqr As Double
        Dim Amp As Double
        Dim StartDelta As Integer
        Dim StartCentre, PicCentre As Integer

        Column = 0
        StartCentre = nStartScreenPoints / 2
        PicCentre = ((nPixelsHigh / 2) * StartMagnification) / StartSpan
        xPerPixelSq = xStepPerPixel * xStepPerPixel
        While (Column <= nPixelsWide) And (StopIt = False)
            lblComplete.Text = String.Format("{0:0.0}%", (Column * 100.0) / nPixelsWide)
            Row = 0
            ColDistSqr = Column * Column * xPerPixelSq
            For nCount = 0 To nDeltas
                PathLens(nCount) = -1
            Next nCount
            While (Row <= MaxRow) And (StopIt = False)
                ZeroPhase(PhaseSum)
                For StartIndex = 0 To nStartScreenPoints
                    If (StartMap(StartIndex).Prob > 0) Then
                        StartDelta = StartIndex - StartCentre
                        StartDelta = StartDelta + PicCentre
                        StartDelta = System.Math.Abs(StartDelta - (Row / yPixelsPerStartStep))
                        If (PathLens(StartDelta) < 0) Then
                            PathLen = StartDelta * yPixelsPerStartStep
                            PathLen = PathLen * PathLen
                            PathLen = PathLen + ColDistSqr
                            PathLen = System.Math.Sqrt(PathLen)
                            PathLens(StartDelta) = PathLen
                            WavePart = TwoPi_Lambda * (PathLen)
                            If (PathLen > 0) Then
                                Amp = 1 / PathLen
                            Else
                                Amp = 1
                            End If
                            Sines(StartDelta) = Amp * System.Math.Sin(WavePart)
                            Cosines(StartDelta) = Amp * System.Math.Cos(WavePart)
                        Else
                            PathLen = PathLens(StartDelta)
                        End If

                        'StartOffs = yPixelsPerStartStep * (StartIndex - (StartCentre))
                        'StartOffs = StartOffs + (nPixelsHigh / 2)
                        'PathLen = (StartOffs - Row)
                        'PathLen = PathLen * PathLen
                        'PathLen = PathLen + ColDistSqr
                        'PathLen = Sqr(PathLen)
                        'If (PathLen > 0) Then
                        '  Amp = 1 / PathLen
                        'Else
                        '  Amp = 1
                        'End If
                        If (PathLen > 0) Then
                            AddPhaseNew(PhaseSum, Sines(StartDelta), Cosines(StartDelta))
                            'Else
                            '   AddPhase PhaseSum, 0, StartMap(StartIndex).Prob
                        End If
                    End If
                Next StartIndex
                System.Windows.Forms.Application.DoEvents()

                Map(Row, Column) = AmplitudeSquared(PhaseSum)
                Map(nPixelsHigh - Row, Column) = Map(Row, Column)
                'If (Column < 1500) Then
                If (Map(Row, Column) >= MapMaxVal(Column, 0)) Then
                    MapMaxVal(Column, 0) = Map(Row, Column)
                    'MapMaxVal(Column, 1) = Map(Row, Column)
                End If
                'End If
                Row = Row + 1
            End While
            'UPGRADE_ISSUE: PicResult was upgraded to a Panel, and cannot be coerced to a PictureBox. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0FF1188E-27C0-4FED-842D-159C65894C9B"'
            ShowMap(PicResult, Column, PrevDisplayColumn)
            PrevDisplayColumn = Column
            Column = Column + 1
        End While

    End Sub

    Private Sub StartRectNew_Click()
        Dim Column, Row, StartIndex As Integer
        Dim WorkingIndex, WorkingRow As Integer
        Dim Amplitude As Double
        Dim StartPerPixel As Integer
        Dim nCount As Short
        Row = 0 : Column = 0 : StartIndex = 0
        StartPerPixel = StartMagnification / StartSpan
        Dim HalfRows As Integer : HalfRows = nPixelsHigh / 2
        Dim StartOffs, PathLen As Double

        'EraseMap
        InitMaps(True)
        StopIt = False
        Dim xPerPixelSq As Double
        ' Dim MaxRow As Long: MaxRow = nPixelsHigh / 2
        Dim ColDistSqr As Double
        Column = 0
        xPerPixelSq = xStepPerPixel * xStepPerPixel
        While (Column <= nPixelsWide) And (StopIt = False)
            Row = 0
            For nCount = 0 To nPixelsHigh
                ZeroPhase(PhaseSum(nCount))
            Next nCount
            ColDistSqr = Column * Column * xPerPixelSq
            StartIndex = nStartScreenPoints
            While (Row <= nPixelsHigh) And (StartIndex >= 0)
                nCount = 1
                While nCount <= StartPerPixel And StartIndex >= 0
                    StartOffs = StartIndex - (nStartScreenPoints / 2)
                    StartOffs = yPixelsPerStartStep * StartOffs
                    StartOffs = StartOffs + (nPixelsHigh / 2)
                    PathLen = (StartOffs - Row)
                    PathLen = PathLen * PathLen
                    PathLen = PathLen + ColDistSqr
                    PathLen = System.Math.Sqrt(PathLen)
                    If (PathLen > 0) Then
                        Amplitude = 1 / PathLen
                    Else
                        Amplitude = 1
                    End If

                    WorkingIndex = StartIndex : WorkingRow = Row
                    ' Back up
                    While (WorkingRow >= 0) And (WorkingIndex >= 0)
                        If (StartMap(WorkingIndex).Prob > 0) And (WorkingRow <= HalfRows) Then
                            AddPhase(PhaseSum(WorkingRow), (TwoPi_Lambda * PathLen), Amplitude)
                        End If
                        WorkingIndex = WorkingIndex - StartPerPixel
                        WorkingRow = WorkingRow - 1
                    End While
                    ' Run Down
                    WorkingIndex = StartIndex + StartPerPixel : WorkingRow = Row + 1
                    While (WorkingRow <= nPixelsHigh) And (WorkingIndex <= nStartScreenPoints)
                        If (StartMap(WorkingIndex).Prob > 0) And (WorkingRow <= HalfRows) Then
                            AddPhase(PhaseSum(WorkingRow), (TwoPi_Lambda * PathLen), Amplitude)
                        End If
                        WorkingIndex = WorkingIndex + StartPerPixel
                        WorkingRow = WorkingRow + 1
                    End While

                    StartIndex = StartIndex - 1
                    nCount = nCount + 1
                End While ' nCount
                Row = Row + 1
                System.Windows.Forms.Application.DoEvents()
            End While
            For nCount = 0 To HalfRows
                Map(nCount, Column) = AmplitudeSquared(PhaseSum(nCount))
                Map(nPixelsHigh - nCount, Column) = Map(nCount, Column)
                If (Map(nCount, Column) >= MapMaxVal(Column, 0)) Then
                    MapMaxVal(Column, 0) = Map(nCount, Column)
                End If
            Next nCount
            ShowMap(PicResult, Column, Column - 1)
            Column = Column + 1
        End While

    End Sub


    Private Sub StartRect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles StartRect.Click
        StartRectNew_Click()
    End Sub

    Private Sub StartRectOld_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles StartRectOld.Click
        StartRect_old_Click()
    End Sub

    Private Sub PicResult_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicResult.Click

    End Sub
    Public Sub ShowMap(ByRef PicCanvas As PictureBox, ByRef MaxCol As Integer, ByRef StartCol As Integer, Optional ByRef nOption As Short = 3)
        Dim Row, MapCol, nCount As Integer
        Dim pVal As Double
        Dim GreenVal, RedVal, BlueVal As Integer
        Dim Val2, Val1, Val3 As Integer
        'Dim RunningMax As Double
        Dim nPeakCol As Integer
        Dim Peak2 As Integer
        Dim nRowUpper As Integer
        Dim nRowLower As Integer
        Dim nMidrow As Integer
        Dim MyColour As New System.Drawing.Color
        If PicCanvas.ClientSize.Width < 1 Or
           PicCanvas.ClientSize.Height < 1 Then Exit Sub

        ' Create the Bitmap and Graphic objects.
        If (MyGraphics Is Nothing) Then
            MyBitmap = New Bitmap(PicCanvas.ClientSize.Width, PicCanvas.ClientSize.Height)
            MyGraphics = Graphics.FromImage(MyBitmap)
        End If
        If (StartCol < 1) Then
            MyGraphics.Clear(Color.Black)
        End If
        PicCanvas.Image = MyBitmap
        PicCanvas.Refresh()

        nPeakCol = nPixelsWide + 1
        Peak2 = nPixelsWide + 2
        'nOption = 0

        If (StartCol < 0) Then StartCol = 0
        If (StartCol <= 10) Then
            RunningMax = MapMaxVal(StartCol, 0)
        Else
            For nCount = 1 To StartCol - 1
                If (nCount > nPeakCol) Then
                    RunningMax = (RunningMax * 999 + MapMaxVal(nCount, 0)) / 1000
                Else
                    RunningMax = (RunningMax + 9 * MapMaxVal(nCount, 0)) / 10
                End If
            Next nCount
        End If

        For MapCol = StartCol To MaxCol
            If (StartCol = 0) Then
                lblComplete.Text = String.Format("{0}:{1:0.0}%", nOption, (MapCol * 100.0) / MaxCol)
            End If

            If ((MapCol And &HF) = &HF) Then System.Windows.Forms.Application.DoEvents()
            If (MapCol > Peak2) Then
                RunningMax = RunningMax
            ElseIf (MapCol > nPeakCol) Then
                RunningMax = (RunningMax * 99 + MapMaxVal(MapCol + 20, 0)) / 100
            ElseIf MapCol < 12 Then
                RunningMax = MapMaxVal(MapCol, 0)
            Else
                RunningMax = (RunningMax * 3 + MapMaxVal(MapCol - 3, 0)) / 4
            End If
            nMidrow = (nPixelsHigh / 2) + 1
            nRowUpper = 0
            nRowLower = nMidrow + nMidrow - 1
            For Row = 0 To nMidrow
                If (Map(Row, MapCol) > 0) Then
                    pVal = ((Map(Row, MapCol)) / RunningMax) * 1.2
                    If (pVal > 1.0#) Then pVal = 1.0#
                    pVal = pVal * 255
                    Val1 = pVal
                    If (Val1 > 255) Then Val1 = 255
                    Val2 = pVal * 4.5
                    If (Val2 > 255) Then Val2 = 255
                    pVal = pVal * 17.0#
                    Val3 = pVal
                    If (Val3 > 255) Then Val3 = 255
                    Select Case nOption
                        Case 0 ' bgr
                            BlueVal = Val3
                            GreenVal = Val2
                            RedVal = Val1
                        Case 1 ' brg
                            BlueVal = Val3
                            RedVal = Val2
                            GreenVal = Val1
                        Case 2 ' grb
                            GreenVal = Val3
                            RedVal = Val2
                            BlueVal = Val1
                        Case 3 ' gbr
                            GreenVal = Val3
                            BlueVal = Val2
                            RedVal = Val1
                        Case 4 ' rgb
                            RedVal = Val3
                            GreenVal = Val2
                            BlueVal = Val1
                        Case 5 ' rbg
                            RedVal = Val3
                            BlueVal = Val2
                            GreenVal = Val1
                        Case 6 ' (br)g
                            BlueVal = Val2
                            RedVal = Val2
                            GreenVal = Val1
                        Case 7 ' b(rg)
                            BlueVal = Val2
                            RedVal = Val1
                            GreenVal = Val1
                        Case 8 ' r(b)
                            RedVal = Val2
                            BlueVal = Val1
                            GreenVal = Val1
                        Case 9 ' g(b)
                            GreenVal = Val2
                            BlueVal = Val1
                            RedVal = Val1
                        Case Else
                            BlueVal = Val3
                            GreenVal = Val2
                            RedVal = Val1
                    End Select
                    MyColour = Color.FromArgb(&HFF, RedVal, GreenVal, BlueVal)
                    'UPGRADE_ISSUE: PictureBox method PicBitmap.PSet was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                    MyBitmap.SetPixel(MapCol, nRowUpper, MyColour)
                    If (nRowLower <> nRowUpper) Then
                        MyBitmap.SetPixel(MapCol, nRowLower, MyColour)
                    End If
                End If
                nRowUpper = nRowUpper + 1
                nRowLower = nRowLower - 1
            Next Row
            PicCanvas.Refresh()
        Next MapCol
        PicCanvas.Refresh()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub lblComplete_Click(sender As Object, e As EventArgs) Handles lblComplete.Click

    End Sub
End Class