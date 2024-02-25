//==================================================================================================
// Script: responsive-menu.js
// Author Jimmie Magnusson
// Description:
//
// This demo script converts regular navigation menus to dropdown menus and back depending on the 
// value of a CSS variable.
//==================================================================================================

//==================================================================================================
// Events
//==================================================================================================

// Subscribe to the document ready event
jQuery(document).ready(
function()
{
  try
  {
    // Create MenuData objects containing the IDs of the menus to convert and the CSS variables that determines when to convert them.
    const convertableMenus = 
    [
      new MenuData("primary-navigation-menu", "--convert-primary-navigation-menu-to-hamburger-menu"),
      new MenuData("transformable-navigation-menu-demo", "--convert-transformable-demo-navigation-menu-to-dropdown-menu")
    ];

    // Create a new handler and pass the menu data
    new DropdownMenuConverter(convertableMenus).Enable();
  }
  catch (exception)
  {
    console.error("responsive-menu.js: An error occurred: " + exception.message)
  }
});

// ==========================================================================
// Class: DropdownMenuConverter
//
// A class that converts regular navigation menus to dropdown menus and 
// back again depending on the value of CSS variables.
// ==========================================================================
class DropdownMenuConverter
{
  // ==========================================================================
  // Fields
  // ==========================================================================

  // An array of menus to convert (MenuData objects)
  #convertableMenus = null;

  // ==========================================================================
  // Constructors
  // ==========================================================================

  // A constructor that takes an array of MenuData objects.
  constructor(convertableMenus)
  {
    this.#convertableMenus = convertableMenus;
  }

  // ==========================================================================
  // Methods
  // ==========================================================================

  // Enables the menu conversion functionality.
  Enable()
  {
    this.#UpdateMenus();
    
    // Can't use the 'this' keyword directly in the click event handler
    let sender = this;

    $(window).on("resize.responsive-menu", function() 
    {
      sender.#UpdateMenus();
    });
  }

  // Converts from a regular navigation menu to a dropdown menu
  #TryConvertToDropdownMenu(menuData)
  {    
    if (!$("#" + menuData.MenuIdString).hasClass("dropdown-menu"))
    {
      // Create the hamburger icon
      let hamburgerIcon = $('<div id="' +  menuData.MenuIdString + '-dropdown-menu-icon" class="dropdown-menu-icon"></div>');
      hamburgerIcon.append('<span class="dropdown-menu-icon-stripe"></span>');
      hamburgerIcon.append('<span class="dropdown-menu-icon-stripe"></span>');
      hamburgerIcon.append('<span class="dropdown-menu-icon-stripe"></span>');
      hamburgerIcon.prependTo("#" + menuData.MenuIdString);

      // Modify primary navigation menu
      $("#" + menuData.MenuIdString).removeClass("horizontal-menu");
      $("#" + menuData.MenuIdString).addClass("dropdown-menu");
      $("#" + menuData.MenuIdString).attr("tabindex", "0");
    }
  }

  // Converts from a dropdown menu to a regular navigation menu
  #TryConvertToRegularMenu(menuData)
  {
    if ($("#" + menuData.MenuIdString).hasClass("dropdown-menu"))
    {
      // Delete the hamburger icon
      $("#" + menuData.MenuIdString + "-dropdown-menu-icon").remove();
      
      // Modify primary navigation menu      
      $("#" + menuData.MenuIdString).removeClass("dropdown-menu");
      $("#" + menuData.MenuIdString).addClass("horizontal-menu");
      $("#" + menuData.MenuIdString).removeAttr("tabindex");
    }
  }

  // Loops all menus and attempts to convert them
  #UpdateMenus()
  {
    this.#convertableMenus.forEach(menuData => 
    {
      // 0 = Convert to regular menu
      // 1 = Convert to dropdown menu
      let value = $(":root").css(menuData.CssCheckVariableName);

      if (value === "1")
      {
        this.#TryConvertToDropdownMenu(menuData);
      }
      else if (value === "0")
      {
        this.#TryConvertToRegularMenu(menuData)
      }
    });
  }
}

// ==========================================================================
// Class: MenuData
//
// A class that holds the ID string of the menu to convert as well as the 
// CSS variable (values 0 or 1) to use to determine when to convert the menu.
// ==========================================================================
class MenuData
{
  // ==========================================================================
  // Public variables
  // ==========================================================================

  // The name of the CSS variable (values 0 or 1) to use to determine when to convert the menu.
  CssCheckVariableName = "";

  // The id string for the navigation menu.
  MenuIdString = "";  

  // ==========================================================================
  // Constructors
  // ==========================================================================

  // A constructor that takes the ID of the menu and the CSS variable name to determine when to convert the menu.
  constructor(menuIdString, cssCheckVariableName)
  {
    this.MenuIdString = menuIdString;
    this.CssCheckVariableName = cssCheckVariableName;
  }
}