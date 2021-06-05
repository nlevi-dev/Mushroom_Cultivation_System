package via.sep4.Viewspecimen;

import android.os.Bundle;

import androidx.cardview.widget.CardView;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import via.sep4.Model.Mushroom;

import androidx.navigation.Navigation;

import via.sep4.Model.SensorData;
import via.sep4.Model.Status;
import via.sep4.Persistence.WebClient;
import via.sep4.Persistence.WebHandler;
import via.sep4.R;

import android.widget.Button;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

/**
 * @author Bogdan Mezei
 * @version 1.0
 */
public class ViewSpecimen extends Fragment {

    Button diaryButton;
    Button stageButton;
    
    CardView temperature;
    CardView humidity;
    CardView CO2;
    CardView light;
    
    ArrayList<SensorData> data = new ArrayList<>();
    
    TextView actualTemp;
    TextView actualHum;
    TextView actualCO2;
    TextView actualLight;
    
    TextView rangeTemp;
    TextView rangeHum;
    TextView rangeCO2;
    TextView rangeLight;
    
    TextView stage;
    TextView MushroomName;
    
    WebHandler webHandler;
    Status status;
    

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    private ViewSpecimenViewModel viewModel;

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;
    private Mushroom mushroom;
    private View v;

    public ViewSpecimen() {
        // Required empty public constructor
    }

    public ViewSpecimen(Mushroom mushroom){
        this.mushroom = mushroom;
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment ViewSpecimen.
     */
    // TODO: Rename and change types and number of parameters
    public static ViewSpecimen newInstance(String param1, String param2) {
        ViewSpecimen fragment = new ViewSpecimen();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        viewModel = new ViewSpecimenViewModel();
        webHandler = new WebHandler();
//        viewModel.getSensorData();
//        final Observer<SensorDataList> sensorDataObserver = new Observer<SensorDataList>()
//        {
//            @Override
//            public void onChanged(@Nullable final SensorDataList sensorDataList)
//            {
//                actualTemp.setText(String.valueOf(sensorDataList.getList().get(sensorDataList.getList().size()).getCurrent_air_temperature()));
//            }
//        };

//        viewModel.getSensorLiveData().observe(this, sensorDataObserver);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View root = inflater.inflate(R.layout.fragment_view_specimen, container, false);
        diaryButton = root.findViewById(R.id.diaryButton);
        stageButton = root.findViewById(R.id.stageButton);
        
        temperature = root.findViewById(R.id.temperature);
        humidity = root.findViewById(R.id.humiditycard);
        CO2 = root.findViewById(R.id.CO2);
        light = root.findViewById(R.id.lightcard);
        
        actualTemp = root.findViewById(R.id.actualTemp);
        actualHum = root.findViewById(R.id.actualHum);
        actualCO2 = root.findViewById(R.id.actualCO2);
        actualLight = root.findViewById(R.id.actualLight);
    
        rangeTemp = root.findViewById(R.id.rangeTemp);
        rangeHum = root.findViewById(R.id.rangeHum);
        rangeCO2 = root.findViewById(R.id.rangeCO2);
        rangeLight = root.findViewById(R.id.rangeLight);
        
        stage = root.findViewById(R.id.stage);
        MushroomName = root.findViewById(R.id.MushroomName);
        
        mushroom = (Mushroom) getArguments().getSerializable("mushroom");
        MushroomName.setText(mushroom.getName());
    
        Call<List<Status>> call = WebClient.getStatusAPI().getSpecimenStatusBySpecimenKey(mushroom.getSpecimen_id());
        call.enqueue(new Callback<List<Status>>()
                     {
    
                         @Override
                         public void onResponse(Call<List<Status>> call, Response<List<Status>> response)
                         {
                             status = response.body().get(response.body().size()-1);
                            stage.setText(response.body().get(response.body().size()-1).getStage_name());
                         }
    
                         @Override
                         public void onFailure(Call<List<Status>> call, Throwable t)
                         {
        
                         }
                     });
        
        WebHandler.setFromDateToDate();
        Call<ArrayList<SensorData>> callsensor = WebClient.getSpecimenAPI().getSpecimenSensor(mushroom.getSpecimen_id(), WebClient.date_from,WebClient.date_to);
        callsensor.enqueue(new Callback<ArrayList<SensorData>>()
        {
            @Override
            public void onResponse(Call<ArrayList<SensorData>> call, Response<ArrayList<SensorData>> response)
            {
                data = response.body();
                actualTemp.setText(String.valueOf(response.body().get(response.body().size()-1).getAir_temperature()));
                actualHum.setText(String.valueOf(response.body().get(response.body().size()-1).getAir_humidity()));
                actualCO2.setText(String.valueOf(response.body().get(response.body().size()-1).getAir_co2()));
                actualLight.setText(String.valueOf(response.body().get(response.body().size()-1).getLight_level()));
                
                if(response.body().get(response.body().size()-1).getAir_temperature() < response.body().get(response.body().size()-1).getDesired_air_temperature() - 3)
                    rangeTemp.setText("Under recommended range");
                else if(response.body().get(response.body().size()-1).getAir_temperature() > response.body().get(response.body().size()-1).getDesired_air_temperature() + 3)
                    rangeTemp.setText("Over recommended range");
    
                if(response.body().get(response.body().size()-1).getAir_humidity() < response.body().get(response.body().size()-1).getDesired_air_humidity() - 3)
                    rangeHum.setText("Under recommended range");
                else if(response.body().get(response.body().size()-1).getAir_humidity() > response.body().get(response.body().size()-1).getDesired_air_humidity() + 3)
                    rangeHum.setText("Over recommended range");
    
                if(response.body().get(response.body().size()-1).getAir_co2() < response.body().get(response.body().size()-1).getDesired_air_co2() - 3)
                    rangeCO2.setText("Under recommended range");
                else if(response.body().get(response.body().size()-1).getAir_co2() > response.body().get(response.body().size()-1).getDesired_air_co2() + 3)
                    rangeCO2.setText("Over recommended range");
    
                if(response.body().get(response.body().size()-1).getLight_level() < response.body().get(response.body().size()-1).getDesired_light_level() - 3)
                    rangeLight.setText("Under recommended range");
                else if(response.body().get(response.body().size()-1).getLight_level() > response.body().get(response.body().size()-1).getDesired_light_level() + 3)
                    rangeLight.setText("Over recommended range");
                
                
            }
        
            @Override
            public void onFailure(Call<ArrayList<SensorData>> call, Throwable t)
            {
                System.out.println(t.getMessage());
            }
        });
        
        
        temperature.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                mushroom = (Mushroom) getArguments().getSerializable("mushroom");
                Bundle bundle = new Bundle();
                bundle.putString("Type","Temperature");
                bundle.putString("Range",(int) (data.get(data.size()-1).getDesired_air_temperature()-3) + "C - " + (int) (data.get(data.size()-1).getDesired_air_temperature()+3) + "C");
                bundle.putString("Name",mushroom.getName());
                bundle.putSerializable("Data",data);
                NavController nav = Navigation.findNavController(root);
                nav.navigate(R.id.action_viewSpecimen_to_visualisation,bundle);
            }
        });
        humidity.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                mushroom = (Mushroom) getArguments().getSerializable("mushroom");
                Bundle bundle = new Bundle();
                bundle.putString("Type","Humidity");
                bundle.putString("Range",(int) (data.get(data.size()-1).getDesired_air_humidity()-3) + " - " + (int) (data.get(data.size()-1).getDesired_air_humidity()+3) + "");
                bundle.putString("Name",mushroom.getName());
                bundle.putSerializable("Data",data);
                NavController nav = Navigation.findNavController(root);
                nav.navigate(R.id.action_viewSpecimen_to_visualisation,bundle);
            }
        });
        CO2.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                mushroom = (Mushroom) getArguments().getSerializable("mushroom");
                Bundle bundle = new Bundle();
                bundle.putString("Type","CO2 Level");
                bundle.putString("Range",(int) (data.get(data.size()-1).getDesired_air_co2()-3) + " - " + (int) (data.get(data.size()-1).getDesired_air_co2()+3) + "");
                bundle.putString("Name",mushroom.getName());
                bundle.putSerializable("Data",data);
                NavController nav = Navigation.findNavController(root);
                nav.navigate(R.id.action_viewSpecimen_to_visualisation,bundle);
            }
        });
        light.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                mushroom = (Mushroom) getArguments().getSerializable("mushroom");
                Bundle bundle = new Bundle();
                bundle.putString("Type","Light Level");
                bundle.putString("Range",(int) (data.get(data.size()-1).getDesired_light_level()-3) + " - " + (int) (data.get(data.size()-1).getDesired_light_level()+3) + "");
                bundle.putString("Name",mushroom.getName());
                bundle.putSerializable("Data",data);
                NavController nav = Navigation.findNavController(root);
                nav.navigate(R.id.action_viewSpecimen_to_visualisation,bundle);
            }
        });
        diaryButton.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                NavController nav = Navigation.findNavController(root);
                nav.navigate(R.id.action_viewSpecimen_to_diary);
            }
        });
        stageButton.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                Bundle bundle = new Bundle();
                bundle.putInt("id", status.getEntry_key());
                bundle.putSerializable("status",status);
                NavController nav = Navigation.findNavController(root);
                nav.navigate(R.id.action_viewSpecimen_to_selectStage,bundle);
            }
        });
        actualTemp.setText("");
        return root;
    }
}