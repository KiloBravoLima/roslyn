' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Runtime.InteropServices

Namespace Microsoft.VisualStudio.LanguageServices.VisualBasic
    ''' <summary>
    ''' A dummy interface with the same GUID as the legacy SID_SVisualBasicCompiler. This is needed so the managed
    ''' package framework can properly expose the service, since we need _some_ type to give for the ProvideService
    ''' attribute. We need our package to have a different GUID than this service, but they need to be implemented by
    ''' the same object since some project systems (Venus) assume we are implemented this way.
    ''' </summary>
    <Guid(Guids.VisualBasicCompilerServiceIdString)>
    Friend Interface IVbCompilerService
    End Interface
End Namespace
