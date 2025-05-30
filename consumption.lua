local timescales = {
    [defines.flow_precision_index.five_seconds] = "5s",
    [defines.flow_precision_index.one_minute] = "1min",
    [defines.flow_precision_index.ten_minutes] = "10min",
    [defines.flow_precision_index.one_hour] = "1h",
    [defines.flow_precision_index.ten_hours] = "10h",
    [defines.flow_precision_index.fifty_hours] = "50h",
    [defines.flow_precision_index.two_hundred_fifty_hours] = "250h",
    [defines.flow_precision_index.one_thousand_hours] = "1000h"
  }
  
  local selected_timescale = {
    defines.flow_precision_index.five_seconds,
    defines.flow_precision_index.ten_minutes
  }
  
  for surface_name, surface in pairs(game.surfaces) do
    local flowdata = game.forces.player.get_item_production_statistics(surface)
    for _, timescale_index in pairs(selected_timescale) do
      local timescale_name = timescales[timescale_index]
      local tbl = {}
      local totals = flowdata.output_counts
      for item_name, _ in pairs(totals) do
        local item_data = {}
        local sample_index = 1
        while true do
          local success, count = pcall(function()
            return flowdata.get_flow_count{
              name = item_name,
              category = "output",
              precision_index = timescale_index,
              sample_index = sample_index,
              count = true
            }
          end)
          if success and count ~= nil then
            item_data["sample_" .. tostring(sample_index)] = count
            sample_index = sample_index + 1
          else
            break
          end
        end
        tbl[item_name] = item_data
      end
      local json_string = "{\n"
      for item_name, item_data in pairs(tbl) do
        json_string = json_string .. "  \"" .. item_name .. "\": {\n"
        local sample_count = 0
        for _ in pairs(item_data) do
          sample_count = sample_count + 1
        end
        local current_sample = 0
        for sample_index, count in pairs(item_data) do
          current_sample = current_sample + 1
          if current_sample == sample_count then
            json_string = json_string .. "    \"" .. sample_index .. "\": " .. tostring(count) .. "\n"
          else
            json_string = json_string .. "    \"" .. sample_index .. "\": " .. tostring(count) .. ",\n"
          end
        end
        json_string = json_string .. "  },\n"
      end
      json_string = json_string .. "}"
      helpers.write_file("consumption-" .. surface_name .. "-" .. timescale_name .. ".json", json_string, false)
    end
  end
  game.player.print("Consumption data exported")
  