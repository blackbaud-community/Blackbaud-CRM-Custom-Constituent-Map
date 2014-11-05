Public Class ConstituentMapCustomUIModel
    Inherits Blackbaud.AppFx.Mapping.UIModel.MapPageCustomMetadataUIModel

    Private Sub ConstituentMapCustomUIModel_Loaded(sender As Object, e As Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs) Handles Me.Loaded

        'add handlers for when the radius search UI is shown and confirmed
        AddHandler Me.RADIUSSEARCH.InvokingAction, AddressOf Me.RadiusSearchInvokingActionHandler
        AddHandler Me.RADIUSSEARCH.CustomFormConfirmed, AddressOf Me.RadiusSearchCustomFormConfirmedHandler

    End Sub

   
    Private Sub RadiusSearchInvokingActionHandler(sender As Object, e As UIModeling.Core.ShowCustomFormEventArgs)

        'Programmatically register an extension to the Radius Search model that is to be shown

        'We're expecting a particular model to be shown here, so as a best practice let's only do work if we encounter the expected model 
        Dim m = TryCast(e.Model, Blackbaud.AppFx.Mapping.UIModel.RadiusSearchCustomMetadataUIModel)
        If m Is Nothing Then Return

        'Instantiate the extension model - note that we use this pattern (as opposed to manually creating the model) in order to ensure that the custom model is correctly instantiated
        Dim args = New CreateCustomUIModelArgs
        args.AssemblyName = "CustomMapping.UIModel.dll"
        args.ClassName = "CustomMapping.UIModel.RadiusSearchExtendedParametersUIModel"
        args.Context = Me.GetRequestContext()
        args.RootModel = Me
        args.TargetBrowser = Me.TargetBrowser
        args.TargetOS = Me.TargetOS

        'Register the extension model as a separate tab on the parent model
        Dim extension = DirectCast(CustomUIModel.CreateModel(args), RadiusSearchExtendedParametersUIModel)
        extension.ExtensionRenderStyle = XmlTypes.DataFormExtensionRenderStyle.Tab
        extension.ExtensionTabCaption = "Parameters"

        'stash a reference to the parameters used to invoke this action so we can use them in the CustomFormConfirmed event
        extension.Tag = e.Parameters

        m.HostedModels.Add(extension)

        'For completeness, invoke the extension model's loaded event
        extension.InvokeModelLoaded()

    End Sub

    Private Sub RadiusSearchCustomFormConfirmedHandler(sender As Object, e As UIModeling.Core.CustomFormConfirmedEventArgs)

        'Based on the custom extension parameter model values specified by the user, programmatically inject additional pushpins on the map.  Note that the OOB model enforces a hard limit of 250 pins which cannot
        'be changed.

        'We are expecting a specific type of model, so as a best practice let's only do work if we encounter the expected model 
        Dim m = TryCast(e.Model, Blackbaud.AppFx.Mapping.UIModel.RadiusSearchCustomMetadataUIModel)
        If m Is Nothing Then Return
        If m.HostedModels.Count = 0 Then Return

        'We are expecting a specific type of extension, so as a best practice let's only do work if we encounter the expected model 
        Dim extension = TryCast(m.HostedModels(0), RadiusSearchExtendedParametersUIModel)
        If extension Is Nothing Then Return

        'No need to do any work if the checkbox field on the extension is unchecked
        'If Not extension.INCLUDENONPRIMARYADDRESSES.Value Then Return

        'grab the parameters used to invoke the radius search action
        Dim radiusSearchParameters = TryCast(extension.Tag, System.Collections.Specialized.NameValueCollection)
        If radiusSearchParameters Is Nothing Then Return

        'grab the current pushpin from the parameters (the pin contains the coordinates to use)
        Dim currentPushPinId = New Guid(radiusSearchParameters.Item("PUSHPINMODELINSTANCEID"))
        Dim currentPushPin = Me.PUSHPINS.GetItemByModelInstanceId(currentPushPinId)
        If currentPushPin Is Nothing Then Return

        'the CUSTOMRADIUS field contains the selected radius value to use
        Dim radiusToUse = m.CUSTOMRADIUS.Value

        'Load the custom proximity data list to fetch the additional addresses that fall within the radius
        Dim rows() = GetDataListRows(currentPushPin.LATITUDE.Value, currentPushPin.LONGITUDE.Value, radiusToUse, extension.ADDRESSTYPE.Value)

        'Finally, iterate over the rows returned by the data list and add pushpins to the map
        If rows IsNot Nothing Then

            'The ID of the Constituent Page
            Dim constitPageId = New Guid("88159265-2B7E-4c7b-82A2-119D01ECD40F")
           
            ' clear out the pushpins created by the inherited MapPageCustomMetadataUIModel before we add your own.  
            Me.PUSHPINS.Value.Clear()
            Dim rowcount As Int16 = 0

            're add the current pushpin since we cleared out all the pushpins above.  
            Me.PUSHPINS.Value.Add(currentPushPin)
            For Each row In rows
                If rowcount = 250 Then Exit For

                Dim pin = Me.PUSHPINS.Value.AddNew()
                pin.RECORDTYPE.Value = "Constituent"
                pin.ADDRESSRECORDTYPE.Value = "Address"
                pin.RECORDID.Value = row.Values(0)
                pin.TITLE.Value = row.Values(1)
                pin.ADDRESSID.Value = row.Values(2)
                pin.ADDRESSBLOCK.Value = row.Values(3)
                pin.CITY.Value = row.Values(4)
                pin.STATE.Value = row.Values(5)
                pin.POSTCODE.Value = row.Values(6)
                pin.COUNTRY.Value = row.Values(7)
                pin.LATITUDE.Value = row.Values(8)
                pin.LONGITUDE.Value = row.Values(9)
                pin.PHONENUMBER.Value = row.Values(11)
                pin.TYPE.Value = 0      '0 = Record, 1 = Pushpin
                pin.COLOR.Value = Blackbaud.AppFx.Mapping.UIModel.MapEditPushpinCustomMetadataUIModel.COLORS.Blue
                pin.NAVIGATIONPAGEID.Value = constitPageId
                rowcount += 1
            Next
        End If

    End Sub

    'The custom proximity data list is loaded in the GetDataListRows() routine:    
    Private Function GetDataListRows(ByVal latitude As Decimal, ByVal longitude As Decimal, ByVal radius As Integer, ByVal addressTypeCodeId As Guid) As Server.DataListResultRow()

        'build a data form item containing the parameters to use
        Dim dfi = New XmlTypes.DataForms.DataFormItem
        dfi.SetValue("LATITUDE", latitude)
        dfi.SetValue("LONGITUDE", longitude)
        dfi.SetValue("DISTANCEMILES", radius)
        dfi.SetValue("ADDRESSTYPE", addressTypeCodeId)
      
        'create and issue the DataListLoad request
        Dim req = New Server.DataListLoadRequest
        req.DataListID = New Guid("9e9822e6-1ca6-4289-b658-b6665f4a129f")   'The ID of the custom proximity data list
        req.ClientAppInfo.TimeOutSeconds = 1200
        req.MaxTotalRecords = 250
        req.Parameters = dfi

        Dim svc = New Server.AppFxWebService(Me.GetRequestContext())

        Try
            Dim reply = svc.DataListLoad(req)
            Return reply.Rows

        Catch ex As Exception
            Throw
            'todo:  write the exception to the event log, or display a prompt to the user etc...
        End Try

        Return Nothing

    End Function


End Class
