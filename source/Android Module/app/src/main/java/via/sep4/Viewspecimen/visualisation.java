package via.sep4.Viewspecimen;

import android.os.Bundle;

import androidx.appcompat.content.res.AppCompatResources;
import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.anychart.AnyChart;
import com.anychart.AnyChartView;
import com.anychart.chart.common.dataentry.DataEntry;
import com.anychart.chart.common.dataentry.ValueDataEntry;
import com.anychart.charts.Cartesian;
import com.anychart.core.cartesian.series.Line;
import com.anychart.data.Mapping;
import com.anychart.data.Set;
import com.anychart.enums.Anchor;
import com.anychart.enums.MarkerType;
import com.anychart.enums.TooltipPositionMode;
import com.anychart.graphics.vector.Stroke;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import via.sep4.Model.SensorData;
import via.sep4.R;
/**
 * @author Bogdan Mezei
 * @version 1.0
 */
public class visualisation extends Fragment
{
	View v;
	TextView name;
	TextView range;
	TextView type;
	TextView current;
	ProgressBar progressBar;
	ArrayList<SensorData> data;
	int MAX_RANGE;
	int MIN_RANGE;
	int ACTUAL_TEMP;
	
	public visualisation()
	{
		// Required empty public constructor
	}
	@Override
	public void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		
	}
	
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
	                         Bundle savedInstanceState) {
		// Inflate the layout for this fragment
		v = inflater.inflate(R.layout.fragment_visualisation, container, false);
		name = v.findViewById(R.id.name);
		range = v.findViewById(R.id.range);
		type = v.findViewById(R.id.type);
		current = v.findViewById(R.id.current);
		name.setText(getArguments().getString("Name"));
		range.setText(getArguments().getString("Range"));
		type.setText(getArguments().getString("Type"));
		data = new ArrayList<SensorData>((ArrayList<SensorData>) getArguments().getSerializable("Data"));
		
		switch (getArguments().getString("Type"))
		{
			case "Temperature":
				ACTUAL_TEMP = (int) data.get(data.size() - 1).getAir_temperature();
				current.setText(ACTUAL_TEMP + "C");
				
				MAX_RANGE = (int) data.get(data.size() - 1).getDesired_air_temperature() + 3;
				MIN_RANGE = (int) data.get(data.size() - 1).getDesired_air_temperature() - 3;
				break;
			case "Humidity":
				ACTUAL_TEMP = (int) data.get(data.size() - 1).getAir_humidity();
				current.setText(ACTUAL_TEMP+"");
				
				MAX_RANGE = (int) data.get(data.size() - 1).getDesired_air_humidity() + 3;
				MIN_RANGE = (int) data.get(data.size() - 1).getDesired_air_humidity() - 3;
				break;
			case "CO2 Level":
				ACTUAL_TEMP = (int) data.get(data.size() - 1).getAir_co2();
				current.setText(ACTUAL_TEMP+"");
				
				MAX_RANGE = (int) data.get(data.size() - 1).getDesired_air_co2() + 3;
				MIN_RANGE = (int) data.get(data.size() - 1).getDesired_air_co2() - 3;
				break;
			default:
				ACTUAL_TEMP = (int) data.get(data.size() - 1).getLight_level();
				current.setText(ACTUAL_TEMP+"");
				
				MAX_RANGE = (int) data.get(data.size() - 1).getDesired_light_level() + 3;
				MIN_RANGE = (int) data.get(data.size() - 1).getDesired_light_level() - 3;
				break;
		}
		
		progressBar = (ProgressBar) v.findViewById(R.id.progressBar);
		progressBar.setMax(MAX_RANGE);
		progressBar.setMin(MIN_RANGE);
		if(ACTUAL_TEMP <= MAX_RANGE && ACTUAL_TEMP >= MIN_RANGE)
		{
			progressBar.setProgress(ACTUAL_TEMP);
		}
		else if(ACTUAL_TEMP > MAX_RANGE)
		{
			progressBar.setProgressDrawable(AppCompatResources.getDrawable(getContext(),R.drawable.circle_over));
			progressBar.setProgress(MAX_RANGE);
		}
		else
		{
			progressBar.setProgressDrawable(AppCompatResources.getDrawable(getContext(),R.drawable.circle_under));
			progressBar.setProgress(MIN_RANGE);
		}
		
		setupCharts();
		
		return v;
	}
	
	private void setupCharts(){
		
		AnyChartView anyChartView1;
		 anyChartView1 = v.findViewById(R.id.anychart_temp_graph);
		
		
		Cartesian cartesian = AnyChart.line();
		
		cartesian.animation(true);
		
		cartesian.padding(10d, 20d, 5d, 20d);
		
		cartesian.crosshair().enabled(true);
		cartesian.crosshair()
				.yLabel(true)
				// TODO ystroke
				.yStroke((Stroke) null, null, null, (String) null, (String) null);
		
		cartesian.tooltip().positionMode(TooltipPositionMode.POINT);
		ArrayList<DataEntry> seriesData = new ArrayList<>();
		
		switch (getArguments().getString("Type"))
		{
			case "Temperature":
			{
				cartesian.title("Temperature Monitor");
				cartesian.yAxis(0).title("Degrees C");
				cartesian.xAxis(0).labels().padding(5d, 5d, 5d, 5d);
				
				String pattern = "HH:mm";
				SimpleDateFormat format = new SimpleDateFormat(pattern);
				for (SensorData data : data)
				{
					Date date = new Date(data.getEntry_time());
					seriesData.add(new CustomDataEntry(format.format(date), data.getAir_temperature()));
				}
				break;
			}
			case "Humidity":
			{
				cartesian.title("Humidity Monitor");
				
				cartesian.yAxis(0).title("Humidity");
				cartesian.xAxis(0).labels().padding(5d, 5d, 5d, 5d);
				
				String pattern = "HH:mm";
				SimpleDateFormat format = new SimpleDateFormat(pattern);
				for (SensorData data : data)
				{
					Date date = new Date(data.getEntry_time());
					seriesData.add(new CustomDataEntry(format.format(date), data.getAir_humidity()));
				}
				break;
			}
			case "CO2 Level":
			{
				cartesian.title("CO2 Level Monitor");
				
				cartesian.yAxis(0).title("CO2 Level");
				cartesian.xAxis(0).labels().padding(5d, 5d, 5d, 5d);
				
				String pattern = "HH:mm";
				SimpleDateFormat format = new SimpleDateFormat(pattern);
				for (SensorData data : data)
				{
					Date date = new Date(data.getEntry_time());
					seriesData.add(new CustomDataEntry(format.format(date), data.getAir_co2()));
				}
				break;
			}
			default:
			{
				cartesian.title("Light Level Monitor");
				
				cartesian.yAxis(0).title("Light Level C");
				cartesian.xAxis(0).labels().padding(5d, 5d, 5d, 5d);
				
				String pattern = "HH:mm";
				SimpleDateFormat format = new SimpleDateFormat(pattern);
				for (SensorData data : data)
				{
					Date date = new Date(data.getEntry_time());
					seriesData.add(new CustomDataEntry(format.format(date), data.getLight_level()));
				}
				break;
			}
		}
		
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
		switch (getArguments().getString("Type"))
		{
			case "Temperature":
			{
				series1.stroke("green");
				break;
			}
			case "Humidity":
			{
				series1.stroke("blue");
				break;
			}
			case "CO2 Level":
			{
				series1.stroke("red");
				break;
			}
			default:
			{
				series1.stroke("black");
				break;
			}
		}
		cartesian.legend().enabled(true);
		cartesian.legend().fontSize(13d);
		cartesian.legend().padding(0d, 0d, 10d, 0d);
		
		anyChartView1.setChart(cartesian);
	}
}

class CustomDataEntry extends ValueDataEntry
{
	
	public CustomDataEntry(String x, Number value) {
		super(x, value);
	}
	
}
