Imports System
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Collections.Specialized
Imports WavLib.Struct
Public Class WaveFileReader

    ' Public Class WaveFileReader :  IDisposable

    Dim reader As BinaryReader
    Dim mainfile As riffChunk
    Dim format As fmtChunk
    Dim fact As factChunk

    Dim data As dataChunk

#Region "General Utilities"

    'WaveFileReader(string) - 2004 July 28
    'A fairly standard constructor that opens a file using the filename supplied to it.

    Public Sub WaveFileReader(filename As String)
        reader = New BinaryReader(New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
    End Sub

    'long GetPosition() - 2004 July 28
    'Returns the current position of the reader's BaseStream.

    Public Function GetPosition() As Long

        Return reader.BaseStream.Position
    End Function

    'string GetChunkName() - 2004 July 29
    'Reads the next four bytes from the file, converts the 
    'char array into a string, And returns it.
    Public Function GetChunkName() As String

        Return New String(reader.ReadChars(4))
    End Function


    'void AdvanceToNext() - 2004 August 2
    'Advances to the next chunk in the file.  This Is fine, 
    'since we only really care about the fmt And data 
    'streams for now.
    Public Sub AdvanceToNext()
        Dim NextOffset As Long = reader.ReadInt32() 'Get next chunk offset
        'Seek to the next offset from current position
        reader.BaseStream.Seek(NextOffset, SeekOrigin.Current)
    End Sub
#End Region
#Region "Header Extraction Methods"

    'WaveFileFormat ReadMainFileHeader - 2004 July 28
    'Read in the main file header.  Not much more to say, really.
    'For XML serialization purposes, I "correct" the dwFileLength
    'field to describe the whole file's length.

    Public Function ReadMainFileHeader() As riffChunk

        mainfile = New riffChunk()
        mainfile.sGroupID = New String(reader.ReadChars(4))
        mainfile.dwFileLength = reader.ReadUInt32() + 8
        mainfile.sRiffType = New String(reader.ReadChars(4))
        Return mainfile
    End Function

    'fmtChunk ReadFormatHeader() - 2004 July 28
    'Again, Not much to say.
    Public Function ReadFormatHeader() As fmtChunk
        format = New fmtChunk()
        format.sChunkID = "fmt "
        format.dwChunkSize = reader.ReadUInt32()
        format.wFormatTag = reader.ReadUInt16()
        format.wChannels = reader.ReadUInt16()
        format.dwSamplesPerSec = reader.ReadUInt32()
        format.dwAvgBytesPerSec = reader.ReadUInt32()
        format.wBlockAlign = reader.ReadUInt16()
        format.dwBitsPerSample = reader.ReadUInt32()
        Return format
    End Function

    'factChunk ReadFactHeader() - 2004 July 28
    'Again, Not much to say.
    Public Function ReadFactHeader() As factChunk
        fact = New factChunk()
        fact.sChunkID = "fact"
        fact.dwChunkSize = reader.ReadUInt32()
        fact.dwNumSamples = reader.ReadUInt32()
        Return fact
    End Function


    'dataChunk ReadDataHeader() - 2004 July 28
    'Again, Not much to say.
    Public Function ReadDataHeader() As dataChunk

        data = New dataChunk()

        data.sChunkID = "data"
        data.dwChunkSize = reader.ReadUInt32()
        data.lFilePosition = reader.BaseStream.Position

        If fact.Equals(Nothing) = False Then
            data.dwNumSamples = fact.dwNumSamples
        Else
            data.dwNumSamples = data.dwChunkSize / (format.dwBitsPerSample / 8 * format.wChannels)
        End If

        'The above could be written as data.dwChunkSize / format.wBlockAlign, but I want to emphasize what the frames look Like.
        data.dwMinLength = (data.dwChunkSize / format.dwAvgBytesPerSec) / 60
        'data.dSecLength = (Double)data.dwChunkSize / (Double)format.dwAvgBytesPerSec) - (Double)data.dwMinLength * 60
        data.dSecLength = (data.dwChunkSize / format.dwAvgBytesPerSec) - data.dwMinLength * 60
        Return data
    End Function
#End Region
#Region "IDisposable Members"

    Public Sub Dispose()

        If IsNothing(reader) = False Then
            reader.Close()
        End If

    End Sub

#End Region





End Class
