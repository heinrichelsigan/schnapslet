package symantec.itools.resources;

import java.util.ListResourceBundle;

public class ResBundle extends ListResourceBundle {
   public Object[][] getContents() {
       return contents;
   }
   static final Object[][] contents = {
       // ScrollingTextBeanInfo
       {"put", "put"},
       {"some", "some"},
       {"text", "text"},
       {"in", "in"},
       {"here", "here"},
       // End ScrollingTextBeanInfo

       // Wizard
       {"wizard_label_previous", "< Back"},
       {"wizard_label_next",     "Next >"},
       {"wizard_label_finish",   "Finish"},
       {"wizard_label_cancel",   "Cancel"},
       {"wizard_label_help",     "Help"},
       // End Wizard
   };
}