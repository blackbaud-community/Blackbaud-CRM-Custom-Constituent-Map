Blackbaud-CRM-Custom-Constituent-Map
====================================

To extend a custom UI Model with an add-in or extension, you can register the add-in or extenison for the parent form that the custom UI Model summons. 
However, if the custom UI Model is housed on a page and does not summon a parent form, extending it is more complicated.

This sample demonstrates how to extend an out-of-the-box custom UI Model that appears on the **Mapping** paeg in Blackbaud CRM.

You can use the **Mapping** page to geographically locate prospects and constituents on an interactive map. The mapping tool allows you to 
create maps with the addresses in your database, and it includes a custom UI Model that allows you to display all the constituents 
within a certain radius. 

To access the **Mapping** page, you click **Constituent map** from the **Constituents** functional area. To conduct a radius search, you 
place a pushpin on the map and then select **Radius search** from the options that appear when you hover the cursor over the pushpin. 
The Select radius screen that appears is an out-of-the-box custom UI Model that allows you to indicate the area to view constituents.

## What You Will Build ##

This customization demonstrates how to extend the out-of-the-box Radius search custom UI Model that is housed on the **Mapping** page and does not summon a parent form. You will create a new tab for the custom UI Model where end users can select the type of address to display on the map. 

## Prerequisites ##

This customization requires familiarity with Data Forms, UI Models, Custom UI Models, and Data Form Add-ins. Specifically, you should refer to the [prerequisites for UI Modelling techniques](http://www.bbdevnetwork.com/blogs/welcome-uimodelers/) and familiarize yourself with the process of [extending a custom UI Model](http://www.bbdevnetwork.com/blogs/extending-a-custom-ui-model/) through its parent form. 

Before you build the customization, you must also set up mapping credentials for your environment with the **Map settings** action on the **Mapping** page in Blackbaud CRM.

##Resources##
* See the [Blackbaud CRM Read Me](https://github.com/blackbaud-community/Blackbaud-CRM/blob/master/README.md). 
* [Step by Step Instructions](http://www.bbdevnetwork.com/blogs/building-a-custom-constituent-map/) for building a custom constituent map
* [Data Forms](https://www.blackbaud.com/files/support/guides/infinitydevguide/infsdk-developer-help.htm#../Subsystems/data-forms/Content/data-forms/welcome-data-forms.htm) Chapter within Developer Guides

##Contributing##

Third-party contributions are how we keep the code samples great. We want to keep it as easy as possible to contribute changes that show others how to do cool things with Blackbaud SDKs and APIs. There are a few guidelines that we need contributors to follow.

For more information, see our [canonical contributing guide](https://github.com/blackbaud-community/Blackbaud-CRM/blob/master/CONTRIBUTING.md) in the Blackbaud CRM repo which provides detailed instructions, including signing the [Contributor License Agreement](http://developer.blackbaud.com/cla).
