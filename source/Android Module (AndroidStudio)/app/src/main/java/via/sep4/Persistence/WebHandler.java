package via.sep4.Persistence;

import java.io.IOException;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import via.sep4.Model.SensorData;
import via.sep4.Model.Specimen;
import via.sep4.Model.User;

public class WebHandler {
    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class connects the application data and the REST client.
     */
    public Specimen getSpecimen(int specimenKey)
    {
        Specimen s = new Specimen();
        try
        {
            s = WebClient.getSpecimenAPI().getSpecimen(specimenKey).execute().body();
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
        return s;
    }

    public void token(User user, Callback<String> booleanCallback)
    {
        String auth = user.getUsername() + ":" + user.getPassword();
        WebClient.token(auth, booleanCallback);
    }

    public float getCurrentSensorData(int hardwareID, String src)
    {
        final float[] ret = {0};
        Call<SensorData> call = WebClient.getHardwareAPI().getHardwareSensorData(hardwareID);
        call.enqueue(new Callback<SensorData>() {
            @Override
            public void onResponse(Call<SensorData> call, Response<SensorData> response) {
                if(response.body() != null)
                {
                    if (src.equals("co2"))
                    {
                        ret[0] = response.body().getAir_co2();
                    }
                    else if (src.equals("light")) {
                        ret[0] = response.body().getLight_level();
                    }
                }
            }

            @Override
            public void onFailure(Call<SensorData> call, Throwable t) {

            }
        });
        return ret[0];
    }
    
    public static void setFromDateToDate()
    {
        if(WebClient.date_from == 0)
            WebClient.date_from = System.currentTimeMillis()-3600000; //from 1 hour ago
        else
        {
            if(WebClient.date_to != 0)
            {
                WebClient.date_from = WebClient.date_to;
            }
        }
        WebClient.date_to = System.currentTimeMillis();
    }
}