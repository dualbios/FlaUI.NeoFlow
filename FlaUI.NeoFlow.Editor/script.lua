UI:log("Starting automation")

local mainWindow = UI:getMainWindow()

local buttonButton = mainWindow:find("button[uid='ButtonsButtonUid']").AsButton()
buttonButton:click()
UI:wait(100)

local buttonItemsControl = mainWindow:find("list[uid='ButtonsListBoxUid']")
local agdsButtons = buttonItemsControl.findAll("button")
for _, button in ipairs(agdsButtons) do
    print("Button: " .. button:GetText())
end

UI:log("Finished automation")