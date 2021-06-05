package via.sep4.Model;

import android.widget.LinearLayout;
import android.widget.TableRow;

import java.io.Serializable;
import java.util.Objects;

public class Mushroom implements Serializable {
    private String name;
    private TableRow parent;
    private LinearLayout container;
    private int specimen_id;

    public Mushroom(String name){
        this.name = name;
    }

    public LinearLayout getContainer() {
        return container;
    }

    public void setContainer(LinearLayout container) {
        this.container = container;
    }

    public TableRow getParent() {
        return parent;
    }

    public void setParent(TableRow parent) {
        this.parent = parent;
    }

    public void setName(String name) {
        this.name = name;
    }
    
    public int getSpecimen_id()
    {
        return specimen_id;
    }
    
    public void setSpecimen_id(int specimen_id)
    {
        this.specimen_id = specimen_id;
    }
    
    public String getName() {
        return name;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Mushroom mushroom = (Mushroom) o;
        return Objects.equals(name, mushroom.name);
    }

    @Override
    public int hashCode() {
        return Objects.hash(name);
    }
}
