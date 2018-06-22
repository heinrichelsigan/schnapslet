package at.area23;

import at.area23.*;

import java.util.ListResourceBundle;

public class ResBundle_de extends ListResourceBundle {
   public Object[][] getContents() {
       return contents;
   }
   static final Object[][] contents = {
       // ScrollingTextBeanInfo
       {"put", "Schreiben"},
       {"some", "Sie"},
       {"text", "Ihren"},
       {"in", "Text"},
       {"here", "hier"},
       // End ScrollingTextBeanInfo

       // Wizard
       {"wizard_label_previous", "< Zur\u00fcck"},
       {"wizard_label_next",     "Weiter >"},
       {"wizard_label_finish",   "Ende"},
       {"wizard_label_cancel",   "Abbrechen"},
       {"wizard_label_help",     "Hilfe"},
       // End Wizard
   };
}

