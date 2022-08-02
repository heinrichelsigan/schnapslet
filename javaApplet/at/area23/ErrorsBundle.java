package at.area23;

import at.area23.*;
import java.util.ListResourceBundle;

public class ErrorsBundle extends ListResourceBundle {
   public Object[][] getContents() {
       return contents;
   }
   static final Object[][] contents = {
       // LOCALIZE THIS

       { "InvalidBevelStyle",          "Invalid BevelStyle: " },
       { "InvalidBevelSize",           "Invalid bevel size: " },
       { "InvalidFrame",               "Invalid Frame: " },
       { "InvalidDirection",           "Invalid direction: " },
       { "InvalidArrowIndent",         "Invalid arrow indent: " },
       { "ErrorLoadingImage",          "Error loading image {0}" },
       { "InvalidImageStyle",          "Invalid image style: " },
       { "ErrorLoadingImageForURL",    "Unable to load image for URL {0}" },
       { "InvalidAlignStyle",          "Invalid AlignStyle: " },
       { "InvalidVerticalAlignStyle",  "Invalid VerticalAlignStyle: " },
       { "ElementAlreadyInMatrix",     "Element already in Matrix" },
       { "ElementNotInMatrix",         "Element row={0} col={1} is not in matrix" },
       { "RowNotInMatrix",             "Row {0} is not in the matrix" },
       { "MustBeGreaterThanCurrentRow","r must be greater than current row: r={0} current row={1}" },
       { "RowTooLarge",                "requested row too large: r={0}" },
       { "InvalidRowNumber",           "{0} is not a valid row number" },
       { "InvalidColumnIndex",         "Column out of range : " },
       { "InvalidAlignment",           "{0} must be either {1}, {2}, or {3}" },
       { "InvalidSelectedRadioButtonIndex", "Invalid SelectedRadioButtonIndex: " },
       { "InvalidCurrentValue",        "Invalid current value: " },
       { "InvalidMaxValue",            "Invalid max value: " },
       { "InvalidMinValue",            "Invalid min value: " },
       { "InvalidSplitType",           "Invalid SplitType: " },
       { "NodeAlreadyExists",          "Node already exists in tree." },
       { "EmptyStrings",               "Empty strings in structure." },
       { "NoRootLevelNode",            "Indented nodes with no root level node." },
       { "NoParent",                   "Node with no immediate parent.  Check indentation for: " },
       { "InvalidTextLocation",        "Invalid text location: " },
       { "NotCellObject",              "Objects to compare must be Cell instances" },

       // END "LOCALIZE THIS"
   };
}