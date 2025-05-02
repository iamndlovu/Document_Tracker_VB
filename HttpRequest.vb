Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net.Http
Imports System.Text

Public Class HttpRequest
    Private ReadOnly _httpClient As New HttpClient()
    Public Async Function GetRequestAsync(url As String) As Task(Of Object)
        Try
            Dim response = Await _httpClient.GetAsync(url)
            response.EnsureSuccessStatusCode()
            Dim responseBody = Await response.Content.ReadAsStringAsync()
            Dim jsonRes = JObject.Parse(responseBody)
            jsonRes("status") = response.StatusCode.ToString()
            Return jsonRes
        Catch ex As HttpRequestException
            Return New With {
                .msg = $"Error: {ex.Message}",
                .status = "Error"
            }
        End Try
    End Function

    Public Async Function PostRequestAsync(url As String, data As Object) As Task(Of Object)
        Try
            Dim json = JsonConvert.SerializeObject(data)
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")
            Dim response = Await _httpClient.PostAsync(url, content)
            response.EnsureSuccessStatusCode()
            Dim responseBody = Await response.Content.ReadAsStringAsync()
            Dim jsonRes = JObject.Parse(responseBody)
            jsonRes("status") = response.StatusCode.ToString()
            Return jsonRes
        Catch ex As HttpRequestException
            Return New With {
                .msg = $"Error: {ex.Message}",
                .status = "Error"
            }
        End Try
    End Function

    Public Async Function GetListAsync(url As String) As Task(Of List(Of Object))
        Dim result As Object = Await GetRequestAsync(url)

        If result("status") = "Error" Then
            Throw New Exception(result.msg)
        End If

        Dim items As List(Of Object) = result("data").ToObject(Of List(Of Object))()
        'MessageBox.Show(result("data").ToString())
        Return items
    End Function

    Public Async Function PostMultipartRequestAsync(url As String, data As Dictionary(Of String, Object)) As Task(Of Object)
        Try
            ' Create a MultipartFormDataContent object
            Dim multipartContent As New MultipartFormDataContent()

            ' Iterate through the data dictionary
            For Each kvp In data
                If TypeOf kvp.Value Is String Then
                    ' Add string data as a form field
                    multipartContent.Add(New StringContent(kvp.Value.ToString()), kvp.Key)
                ElseIf TypeOf kvp.Value Is Byte() Then
                    ' Add byte array data as a file
                    Dim fileContent As New ByteArrayContent(DirectCast(kvp.Value, Byte()))
                    fileContent.Headers.ContentType = New System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream")
                    multipartContent.Add(fileContent, kvp.Key, kvp.Key) ' Use the key as the file name
                ElseIf TypeOf kvp.Value Is IO.FileInfo Then
                    ' Add file data from a FileInfo object
                    Dim fileInfo As IO.FileInfo = DirectCast(kvp.Value, IO.FileInfo)
                    Dim fileBytes As Byte() = IO.File.ReadAllBytes(fileInfo.FullName)
                    Dim fileContent As New ByteArrayContent(fileBytes)
                    fileContent.Headers.ContentType = New System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream")
                    multipartContent.Add(fileContent, kvp.Key, fileInfo.Name)
                End If
            Next

            ' Send the POST request
            Dim response = Await _httpClient.PostAsync(url, multipartContent)
            response.EnsureSuccessStatusCode()

            ' Read and parse the response
            Dim responseBody = Await response.Content.ReadAsStringAsync()
            Dim jsonRes = JObject.Parse(responseBody)
            jsonRes("status") = response.StatusCode.ToString()
            Return jsonRes
        Catch ex As HttpRequestException
            Return New With {
            .msg = $"Error: {ex.Message}",
            .status = "Error"
        }
        End Try
    End Function

End Class


