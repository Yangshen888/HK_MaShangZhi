<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.messageboard.query.totalCount"
        :pageSize="stores.messageboard.query.pageSize"
        :currentPage="stores.messageboard.query.currentPage"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.messageboard.query.kw"
                      placeholder="输入内容/类型搜索..."
                      @on-search="handleSearchMessageboard()"
                    >
                      <!--                      <Select-->
                      <!--                        slot="prepend"-->
                      <!--                        v-model="stores.messageboard.query.isDeleted"-->
                      <!--                        @on-change="handleSearchMessageboard"-->
                      <!--                        placeholder="删除状态"-->
                      <!--                        style="width:60px;"-->
                      <!--                      >-->
                      <!--                        <Option-->
                      <!--                          v-for="item in stores.messageboard.sources.isDeletedSources"-->
                      <!--                          :value="item.value"-->
                      <!--                          :key="item.value"-->
                      <!--                        >{{item.text}}</Option>-->
                      <!--                      </Select>-->
                      <Select
                        slot="prepend"
                        v-model="stores.messageboard.query.state"
                        @on-change="handleSearchMessageboard"
                        placeholder="发布状态"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in stores.messageboard.sources.releaseStateSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.messageboard.data"
          :columns="stores.messageboard.columns"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{row,index}" slot="state">
            <span>{{renderState(row.state)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="messageDate">
            <span>{{renderDate(row.messageDate)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="responseDate">
            <span>{{renderDate(row.responseDate)}}</span>
          </template>

          <!--          <template slot-scope="{ row, index }" slot="detail">-->
          <!--            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">-->
          <!--              <Button-->
          <!--                type="primary"-->
          <!--                size="small"-->
          <!--                shape="circle"-->
          <!--                icon="md-search"-->
          <!--                @click="handleShow(row)"-->
          <!--              ></Button>-->
          <!--            </Tooltip>-->
          <!--          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Tooltip placement="top" content="回复" :delay="1000" :transfer="true">
              <Button
                v-if="row.state==0"
                v-can="'response'"
                type="primary"
                size="small"
                shape="circle"
                icon="logo-twitch"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="500"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form
        v-if="formModel.opened"
        :model="formModel.fields"
        ref="formPlan"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="留言内容">
              <Input
                type="textarea"
                :autosize="true"
                v-model="formModel.fields.content"
                style="width: 400px"
                :readonly="true"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="留言类型">
              <Input v-model="formModel.fields.type" style="width: 400px" :readonly="true" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="留言日期">
              <Input v-model="liuyanDate" style="width: 400px" :readonly="true" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="回复内容" prop="response">
              <Input
                type="textarea"
                :autosize="true"
                v-model="formModel.fields.response"
                style="width: 400px"
                :maxlength="100"
              />
            </FormItem>
          </Col>
        </Row>
      </Form>

      <div style="margin-top: 100px">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitPlan">回 复</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import Tables from "_c/tables";
import {
  getMessageBoardList,
  loadMessageboard,
  editMessageboard
} from "@/api/messagefeedback/messageboard";
export default {
  name: "messageboard",
  components: {
    Tables,
    DzTable
  },
  data() {
    let checkNum = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入"));
      } else if (value <= 0 || value > 999) {
        callback(new Error("请输入1-999的数字"));
      } else {
        callback();
      }
    };
    return {
      showdetails: false,
      details: "",
      liuyanDate: "",
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        forbidden: { name: "forbidden", title: "禁用" },
        normal: { name: "normal", title: "启用" }
      },
      formModel: {
        opened: false,
        title: "创建类别",
        mode: "create",
        selection: [],
        fields: {
          response: ""
        },
        rules: {
          response: [
            {
              type: "string",
              required: true,
              message: "请输入回复内容",
              trigger: "blur"
            }
          ]
        }
      },
      stores: {
        messageboard: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            year: "",
            isDeleted: 0,
            status: -1,
            state: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {
            releaseStateSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "未回复" },
              { value: 1, text: "已回复" }
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" }
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "留言内容", key: "content", ellipsis: true },
            { title: "留言类型", key: "type", ellipsis: true },
            {
              title: "留言日期",
              key: "messageDate",
              ellipsis: true,
              slot: "messageDate"
            },
            { title: "回复内容", key: "response", ellipsis: true },
            {
              title: "回复时间",
              key: "responseDate",
              ellipsis: true,
              slot: "responseDate"
            },
            { title: "回复状态", key: "state", slot: "state" },
            {
              title: "操作",
              fixed: "right",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static"
      },
      initdatacopy: {
        unit: "",
        unitName: "",
        require: "",
        site: "",
        num: "",
        releaseState: 0
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "edit") {
        return "回复";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.messageUuid);
    }
  },
  methods: {
    renderState(isEnable) {
      let _status = "未知";
      switch (isEnable) {
        case 0:
          _status = "未回复";
          break;
        case 1:
          _status = "已回复";
          break;
      }
      return _status;
    },
    renderDate(date) {
      return this.$utils.Time(date);
    },

    loadMessageboardList() {
      getMessageBoardList(this.stores.messageboard.query).then(res => {
        this.stores.messageboard.data = res.data.data;
        this.stores.messageboard.query.totalCount = res.data.totalCount;
        //console.log(this.stores.educaplan.data);
      });
    },
    handleSearchMessageboard() {
      this.stores.messageboard.query.currentPage = 1;
      this.loadMessageboardList();
    },
    handleRefresh() {
      this.stores.messageboard.query.currentPage = 1;
      this.loadMessageboardList();
    },
    //创建，修改
    handleSubmitPlan() {
      let valid = this.validateForm();
      if (valid) {
        if (this.formModel.mode === "edit") {
          this.doEditPlan();
        }
      }
    },

    doEditPlan() {
      editMessageboard(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadMessageboardList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateForm() {
      let _valid = false;
      this.$refs["formPlan"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },

    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },

    //控制弹出子窗体
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    //编辑
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormRole();
      this.doLoadMessageboard(row.messageUuid);
    },

    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleResetFormRole();
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
      this.handleOpenFormWindow();
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleSwitchFormModeToShow() {
      this.showdetails = true;
    },
    handleResetFormRole() {
      this.liuyanDate = "";
      this.formModel.fields = JSON.parse(JSON.stringify(this.initdatacopy));
      //this.$refs["formPlan"].resetFields();
    },
    doLoadMessageboard(guid) {
      loadMessageboard({ guid: guid }).then(res => {
        this.formModel.fields = res.data.data;
        this.liuyanDate = this.renderDate(this.formModel.fields.messageDate);
      });
    },
    handlePageChanged(page) {
      this.stores.messageboard.query.currentPage = page;
      this.loadMessageboardList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.messageboard.query.pageSize = pageSize;
      this.loadMessageboardList();
    }
  },
  mounted() {
    this.loadMessageboardList();
  }
};
</script>
<style scoped>
</style>
