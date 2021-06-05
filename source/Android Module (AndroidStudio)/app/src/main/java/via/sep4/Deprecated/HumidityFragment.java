package via.sep4.Deprecated;
import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.anychart.AnyChart;
import com.anychart.AnyChartView;
import com.anychart.chart.common.dataentry.DataEntry;
import com.anychart.charts.Cartesian;
import com.anychart.core.cartesian.series.Line;
import com.anychart.data.Mapping;
import com.anychart.data.Set;
import com.anychart.enums.Anchor;
import com.anychart.enums.MarkerType;
import com.anychart.enums.TooltipPositionMode;
import com.anychart.graphics.vector.Stroke;

import java.util.ArrayList;
import java.util.List;

import via.sep4.R;
;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link HumidityFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
@Deprecated
public class HumidityFragment extends Fragment {
    View v;
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public HumidityFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment HumidityFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static HumidityFragment newInstance(String param1, String param2) {
        HumidityFragment fragment = new HumidityFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        v= inflater.inflate(R.layout.fragment_humidity, container, false);
        setupCharts();
        return v;
    }

    private void setupCharts(){
        AnyChartView anyChartView1 = v.findViewById(R.id.anychart_hum_graph);
        anyChartView1.setProgressBar(v.findViewById(R.id.prog_hum_graph));

        Cartesian cartesian = AnyChart.line();

        cartesian.animation(true);

        cartesian.padding(10d, 20d, 5d, 20d);

        cartesian.crosshair().enabled(true);
        cartesian.crosshair()
                .yLabel(true)
                // TODO ystroke
                .yStroke((Stroke) null, null, null, (String) null, (String) null);

        cartesian.tooltip().positionMode(TooltipPositionMode.POINT);

        cartesian.title("Humidity Monitor");

        cartesian.yAxis(0).title("Humidity %");
        cartesian.xAxis(0).labels().padding(5d, 5d, 5d, 5d);

        List<DataEntry> seriesData = new ArrayList<>();
//        seriesData.add(new CustomDataEntry("14May-21:10", 100));
//        seriesData.add(new CustomDataEntry("14May-21:20", 80));
//        seriesData.add(new CustomDataEntry("14May-21:30", 70));
//        seriesData.add(new CustomDataEntry("14May-21:40", 60));
//        seriesData.add(new CustomDataEntry("14May-21:50", 50));
//        seriesData.add(new CustomDataEntry("14May-22:00", 40));
//        seriesData.add(new CustomDataEntry("14May-22:10", 30));
//        seriesData.add(new CustomDataEntry("14May-22:20", 30));
//        seriesData.add(new CustomDataEntry("14May-22:30", 20));
//        seriesData.add(new CustomDataEntry("14May-22:40", 10));
//        seriesData.add(new CustomDataEntry("14May-22:50", 9));

        Set set = Set.instantiate();
        set.data(seriesData);
        Mapping series1Mapping = set.mapAs("{ x: 'x', value: 'value' }");

        Line series1 = cartesian.line(series1Mapping);
        series1.name("Mushroom");
        series1.hovered().markers().enabled(true);
        series1.hovered().markers()
                .type(MarkerType.CIRCLE)
                .size(4d);
        series1.tooltip()
                .position("right")
                .anchor(Anchor.LEFT_CENTER)
                .offsetX(5d)
                .offsetY(5d);
        cartesian.legend().enabled(true);
        cartesian.legend().fontSize(13d);
        cartesian.legend().padding(0d, 0d, 10d, 0d);

        anyChartView1.setChart(cartesian);
    }
}