package via.sep4.Model;

import java.util.ArrayList;

public class SensorDataList
{
    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class is a list class for all sensor data associated with a given specimen.
     */

    private ArrayList<SensorData> sensorData;
    private int specimenKey;

    public SensorDataList(int specimenKey)
    {
        this.specimenKey = specimenKey;
        sensorData = new ArrayList<>();
        //when needed, write retrieval methods - from webservice or local persistence
    }

    public ArrayList<SensorData> getList()
    {
        return sensorData;
    }

    public void addToList(SensorData s)
    {
        sensorData.add(s);
    }

    public int getSpecimenKey()
    {
        return specimenKey;
    }
}
