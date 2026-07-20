// Source - https://stackoverflow.com/a/695517
// Posted by Reverend Gonzo
// Retrieved 2026-03-07, License - CC BY-SA 2.5
package at.area23.schnapsen;

import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Insets;
import java.awt.print.PageFormat;
import java.awt.print.Printable;
import java.awt.print.PrinterException;
import java.io.File;

import javax.swing.ImageIcon;
import javax.swing.JComponent;
import javax.swing.SwingConstants;

public class ImagePanel extends JComponent implements SwingConstants, Printable {

    private Image image;
    private int verticalAlignment = CENTER;
    private int horizontalAlignment = CENTER;

    public ImagePanel() {}

    public ImagePanel(Image image) {
        setImage(image);
    }

    public Image getImage() {
        return image;
    }

    public void setImage(Image image) {
        this.image = image;
        repaint();
    }

    public void setImage(String file) {
        setImage(new ImageIcon(file).getImage());
    }

    public void setImage(File file) {
        setImage(new ImageIcon(file.getAbsolutePath()).getImage());
    }

    public void setImage(byte[] imageData) {
        setImage(imageData==null ? null : new ImageIcon(imageData).getImage());
    }

    public int getVerticalAlignment() {
        return verticalAlignment;
    }

    /**
     * @beaninfo
     *        bound: true
     *         enum: TOP    SwingConstants.TOP
     *               CENTER SwingConstants.CENTER
     *               BOTTOM SwingConstants.BOTTOM
     *    attribute: visualUpdate true
     *  description: The alignment of the image along the Y axis.  
     */
    public void setVerticalAlignment(int verticalAlignment) {
        if( (verticalAlignment==TOP) || (verticalAlignment==CENTER) || (verticalAlignment==BOTTOM) )
            this.verticalAlignment = verticalAlignment;
        else
            throw new IllegalArgumentException("Invalid Vertical Alignment: " + verticalAlignment);
    }

    public int getHorizontalAlignment() {
        return horizontalAlignment;
    }

    /**
     * @beaninfo
     *        bound: true
     *         enum: LEFT    SwingConstants.LEFT
     *               CENTER SwingConstants.CENTER
     *               RIGHT SwingConstants.RIGHT
     *    attribute: visualUpdate true
     *  description: The alignment of the image along the X axis.  
     */
    public void setHorizontalAlignment(int horizontalAlignment) {
        if( (horizontalAlignment==LEFT) || (horizontalAlignment==CENTER) || (horizontalAlignment==RIGHT) )
            this.horizontalAlignment = horizontalAlignment;
        else
            throw new IllegalArgumentException("Invalid Horizontal Alignment: " + horizontalAlignment);
    }

    @Override
    public Dimension getPreferredSize() {
        if(image == null)
            return super.getPreferredSize();
        else
            return new Dimension(image.getWidth(this), image.getHeight(this));
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);

        if(image==null)
            return;

        Insets insets = getInsets();
        int x = insets.left;
        int y = insets.top;

        int w = getWidth() - insets.left - insets.right;
        int h = getHeight() - insets.top - insets.bottom;

        int src_w = image.getWidth(null);
        int src_h = image.getHeight(null);

        double scale_x = ((double)w)/src_w;
        double scale_y = ((double)h)/src_h;

        double scale = Math.min(scale_x, scale_y);

        int dst_w = (int)(scale * src_w);
        int dst_h = (int)(scale * src_h);

        int dx = x + (w-dst_w)/2;
        if(horizontalAlignment==LEFT)
            dx = x;
        else if(horizontalAlignment==RIGHT)
            dx = x + w - dst_w; 

        int dy = y + (h-dst_h)/2;
        if(verticalAlignment==TOP)
            dy = y;
        else if(verticalAlignment==BOTTOM)
            dy = y + h - dst_h; 

        g.drawImage(image, dx, dy, dx+dst_w, dy+dst_h, 0, 0, src_w, src_h, null);
    }


    public int print(Graphics graphics, PageFormat pageFormat, int pageIndex) throws PrinterException {

        if(pageIndex>0 || image==null)
            return NO_SUCH_PAGE;

        double w = pageFormat.getImageableWidth();
        double h = pageFormat.getImageableHeight();

        int src_w = image.getWidth(null);
        int src_h = image.getHeight(null);

        double scale_x = w/src_w;
        double scale_y = h/src_h;

        double scale = Math.min(scale_x, scale_y);

        int dst_w = (int)(scale * src_w);
        int dst_h = (int)(scale * src_h);

        int dx = (int)((w-dst_w)/2);

        int dy = (int)((h-dst_h)/2);

        graphics.drawImage(image, dx, dy, dx+dst_w, dy+dst_h, 0, 0, src_w, src_h, null);

        return PAGE_EXISTS;
    }

}

