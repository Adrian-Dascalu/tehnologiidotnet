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

local selected_timescale = {timescales[defines.flow_precision_index.five_seconds], timescales[defines.flow_precision_index.ten_minutes]}

local single_timescale = timescales[defines.flow_precision_index.ten_minutes]

for surface_name, _ in pairs(game.surfaces) do
  local flowdata = game.forces.player.get_item_production_statistics(surface_name)
  for timescale_index, timescale_name in pairs(selected_timescale) do
    local tbl = {}
    local totals = flowdata.input_counts
    for item_name, _ in pairs(totals) do
      local item_data = {}
      local sample_index = 1
      local error_occurred = false
      while not error_occurred do
        local count
        pcall(function()
          count = flowdata.get_flow_count{name=item_name, category="input", precision_index=timescale_index, sample_index=sample_index, count=true}
        end)
        if count ~= nil then
          item_data["sample_" .. tostring(sample_index)] = count
          sample_index = sample_index + 1
        else
          error_occurred = true
        end
      end
      tbl[item_name] = item_data
    end
    local json_string = "{\n"
    for item_name, item_data in pairs(tbl) do
      json_string = json_string .. "  \"" .. item_name .. "\": {\n"
      for sample_index, count in pairs(item_data) do
        if sample_index == "sample_300" then
          json_string = json_string .. "    \"" .. sample_index .. "\": " .. tostring(count) .. "\n"
        else
          json_string = json_string .. "    \"" .. sample_index .. "\": " .. tostring(count) .. ",\n"
        end
      end
      if item_name == "gun-turret" then
        json_string = json_string .. "  } \n"
      else
        json_string = json_string .. "  },\n"
      end
    end
    json_string = json_string .. "}"
    helpers.write_file("production-" .. surface_name .. "-" .. timescale_name .. ".json", json_string, false)
  end
end
game.player.print("Production data exported")
